using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Scraper.Domain;
using Scraper.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Scraper.Infrastructure.Mongo
{
    public sealed class ShowsRepository : IShowRepository
    {
        private readonly IMongoCollection<Show> _mongoCollection;

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

        public async Task<IEnumerable<Show>> GetAsync(int skip, int take, CancellationToken token)
        {
            if(take == 0)
                return new Show[0];

            var filterBuilder = new FilterDefinitionBuilder<Show>();
            var findFluent = _mongoCollection.Find(filterBuilder.Empty).Limit(take).Skip(skip);

            return await findFluent.ToListAsync(token);
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

            var lastAddedShow = await findFluent.FirstOrDefaultAsync(cancellationToken: token);
            return lastAddedShow?.Id ?? -1;
        }
    }
}