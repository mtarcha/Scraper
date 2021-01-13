using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Scraper.Clients.TvMaze.Models;

namespace Scraper.Clients.TvMaze
{
    public interface ITvMazeApiClient
    {
        Task<IEnumerable<long>> GetShowIdsAsync(long pageNumber, CancellationToken token);
        Task<Show> GetShowAsync(long id, CancellationToken token);
    }
}