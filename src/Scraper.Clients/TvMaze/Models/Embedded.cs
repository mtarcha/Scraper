using System.Collections.Generic;

namespace Scraper.Clients.TvMaze.Models
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