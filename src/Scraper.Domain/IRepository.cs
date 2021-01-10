using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Scraper.Domain
{
    public interface IRepository<TModel>
    {
        Task AddAsync(TModel model, CancellationToken token);

        Task<IEnumerable<TModel>> GetAsync(int skip, int take, CancellationToken token);

        Task<long> CountAsync(CancellationToken token);
    }
}