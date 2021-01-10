using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Scraper.Application.Clients.TvMaze.Models;

namespace Scraper.Application.Clients.TvMaze
{
    public class TvMazeApiClient : ITvMazeApiClient
    {
        private readonly HttpClient _httpClient;

        public TvMazeApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<long>> GetShowIdsAsync(long pageNumber, CancellationToken token)
        {
            var path = $"shows?page={pageNumber}";
            var response = await _httpClient.GetAsync(path, token);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return new long[0];

            var jsonString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeAnonymousType(jsonString, new []{ new { Id = (long)0 }}).Select(x => x.Id).ToList();
        }

        public async Task<Show> GetShowAsync(long id, CancellationToken token)
        {
            var path = $"shows/{id}?embed=cast";
            var response = await _httpClient.GetAsync(path, token);

            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Show>(jsonString);
        }
    }
}