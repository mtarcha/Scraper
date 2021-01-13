using MediatR;

namespace Scraper.Application.Queries
{
    public sealed class GetShowsCountQuery : IRequest<long>
    {
    }
}