using MediatR;
using Scraper.Domain;
using Scraper.Domain.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Scraper.Clients.TvMaze;

namespace Scraper.Application.Commands
{
    public sealed class AddNewShowsCommandHandler : IRequestHandler<AddNewShowsCommand>
    {
        private const int ShowsPerPage = 250;

        private readonly ITvMazeApiClient _tvMazeApiClient;
        private readonly IShowRepository _repository;

        public AddNewShowsCommandHandler(
            ITvMazeApiClient tvMazeApiClient,
            IShowRepository repository)
        {
            _tvMazeApiClient = tvMazeApiClient;
            _repository = repository;
        }

        public async Task<Unit> Handle(AddNewShowsCommand request, CancellationToken cancellationToken)
        {
            var lastAddedShowId = await _repository.GetLastAddedShowIdAsync(cancellationToken);
            var page = lastAddedShowId / ShowsPerPage;

            while (true)
            {
                var showIds = await _tvMazeApiClient.GetShowIdsAsync(page, cancellationToken);

                foreach (var nextShowId in showIds.Where(x => x > lastAddedShowId))
                {
                    var show = await _tvMazeApiClient.GetShowAsync(nextShowId, cancellationToken);

                    await _repository.AddAsync(new Show
                    {
                        Id = show.Id,
                        Name = show.Name,
                        Cast = show.Embedded?.Cast?.OrderByDescending(x => x.Person.Birthday).Select(c => new Person
                        {
                            Id = c.Person.Id,
                            Name = c.Person.Name,
                            Birthday = c.Person.Birthday?.ToString("yyyy-MM-dd")
                        })
                    }, cancellationToken);

                    if (cancellationToken.IsCancellationRequested)
                        return Unit.Value;
                }

                if (cancellationToken.IsCancellationRequested || !showIds.Any())
                    return Unit.Value;

                page++;
            }
        }
    }
}