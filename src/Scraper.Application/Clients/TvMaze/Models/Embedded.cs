using System.Collections.Generic;

namespace Scraper.Application.Clients.TvMaze.Models
{
    public class Embedded
    {
        public IEnumerable<Cast> Cast { get; set; }
    }

    public class Cast
    {
        public Person Person { get; set; }
    }
}