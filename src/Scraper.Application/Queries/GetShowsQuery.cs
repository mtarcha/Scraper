using System.Collections.Generic;
using MediatR;
using Scraper.Domain.Models;

namespace Scraper.Application.Queries
{
    public class GetShowsQuery : IRequest<IEnumerable<Show>>
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}