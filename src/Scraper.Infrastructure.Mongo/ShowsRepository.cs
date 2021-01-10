using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Scraper.Domain;
using Scraper.Domain.Models;

namespace Scraper.Infrastructure.Mongo
{
    public class ShowsRepository : IShowRepository
    {
        private IMongoCollection<Show> _mongoCollection;

        public ShowsRepository(IOptions<ScraperDatabaseSettings> options)
        {
            var settings = options.Value;
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _mongoCollection = database.GetCollection<Show>(settings.ShowsCollectionName);
        }

        public async Task AddAsync(Show model, CancellationToken token)
        {
            await _mongoCollection.InsertOneAsync(model, cancellationToken: token);
        }

        public Task<IEnumerable<Show>> GetAsync(int skip, int take, CancellationToken token)
        {
            throw new System.NotImplementedException();
        }

        public async Task<long> CountAsync(CancellationToken token)
        {
            var filterBuilder = new FilterDefinitionBuilder<Show>();
            return await _mongoCollection.CountDocumentsAsync(filterBuilder.Empty, cancellationToken: token);
        }

        public async Task<long> GetLastAddedShowIdAsync(CancellationToken token)
        {
            var filterBuilder = new FilterDefinitionBuilder<Show>();
            var findFluent = _mongoCollection.Find(filterBuilder.Empty).SortByDescending(x => x.Id).Limit(1);

            var lastAddedShow = await findFluent.FirstAsync(cancellationToken: token);
            return lastAddedShow.Id;
        }
    }
}