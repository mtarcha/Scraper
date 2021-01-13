using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Scraper.Domain;

namespace Scraper.Application.Queries
{
    public sealed class GetShowsCountQueryHandler : IRequestHandler<GetShowsCountQuery, long>
    {
        private readonly IShowRepository _showRepository;

        public GetShowsCountQueryHandler(IShowRepository showRepository)
        {
            _showRepository = showRepository;
        }

        public async Task<long> Handle(GetShowsCountQuery request, CancellationToken cancellationToken)
        {
            return await _showRepository.CountAsync(cancellationToken);
        }
    }
}