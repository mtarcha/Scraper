using System.Collections;
using System.Collections.Generic;

namespace Scraper.Domain.Models
{
    public class Show
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<Person> Cast { get; set; }
    }
}