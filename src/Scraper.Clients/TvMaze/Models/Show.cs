using Newtonsoft.Json;

namespace Scraper.Clients.TvMaze.Models
{
    public class Show
    {
        public long Id { get; set; }

        public string Name { get; set; }

        [JsonProperty("_embedded")]
        public Embedded Embedded { get; set; }
    }
}
