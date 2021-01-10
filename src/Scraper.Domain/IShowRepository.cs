using System.Threading;
using System.Threading.Tasks;
using Scraper.Domain.Models;

namespace Scraper.Domain
{
    public interface IShowRepository : IRepository<Show>
    {
        Task<long> GetLastAddedShowIdAsync(CancellationToken token);
    }
}