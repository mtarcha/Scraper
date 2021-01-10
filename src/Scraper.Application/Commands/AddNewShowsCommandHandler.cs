using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Scraper.Application.Clients.TvMaze;
using Scraper.Domain;
using Scraper.Domain.Models;

namespace Scraper.Application.Commands
{
    public class AddNewShowsCommandHandler : IRequestHandler<AddNewShowsCommand>
    {
        private const int ShowsPerPage = 250;

        private readonly ITvMazeApiClient _tvMazeApiClient;
        private readonly IShowRepository _repository;
        private readonly ILogger<AddNewShowsCommandHandler> _logger;

        public AddNewShowsCommandHandler(
            ITvMazeApiClient tvMazeApiClient,
            IShowRepository repository,
            ILogger<AddNewShowsCommandHandler> logger)
        {
            _tvMazeApiClient = tvMazeApiClient;
            _repository = repository;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddNewShowsCommand request, CancellationToken cancellationToken)
        {
            var lastAddedShowId = await _repository.GetLastAddedShowIdAsync(cancellationToken);
            var page = lastAddedShowId / ShowsPerPage;

            while (true)
            {
                var showIds = await _tvMazeApiClient.GetShowIdsAsync(page, cancellationToken);

                if(!showIds.Any())
                    break;

                foreach (var nextShowId in showIds.Where(x => x > lastAddedShowId))
                {
                    try
                    {
                        var show = await _tvMazeApiClient.GetShowAsync(nextShowId, cancellationToken);

                        await _repository.AddAsync(new Show
                        {
                            Id = show.Id,
                            Name = show.Name,
                            Cast = show.Embedded?.Cast?.Select(c => new Person
                            {
                                Id = c.Person.Id,
                                Name = c.Person.Name,
                                Birthday = c.Person.Birthday
                            }).OrderByDescending(x => x.Birthday)
                        }, cancellationToken);

                        if (cancellationToken.IsCancellationRequested)
                            break;
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "Failed fetch show data from TvMaze");
                    }
                }

                if (cancellationToken.IsCancellationRequested)
                    break;

                page++;
            }

            return Unit.Value;
        }
    }
}