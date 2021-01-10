using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Scraper.Domain;
using Scraper.Domain.Models;

namespace Scraper.Application.Queries
{
    public class GetShowsQueryHandler : IRequestHandler<GetShowsQuery, IEnumerable<Show>>
    {
        private readonly IShowRepository _repository;

        public GetShowsQueryHandler(IShowRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Show>> Handle(GetShowsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAsync(request.Skip, request.Take, cancellationToken);
        }
    }
}