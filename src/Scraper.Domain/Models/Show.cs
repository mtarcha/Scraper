using System.Collections.Generic;

namespace Scraper.Domain.Models
{
    public class Show
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<Person> Cast { get; set; }
    }
}