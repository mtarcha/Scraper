using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scraper.Domain;

namespace Scraper.Infrastructure.Mongo
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMongoDbRepository(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ScraperDatabaseSettings>(
                configuration.GetSection(nameof(ScraperDatabaseSettings)));

            services.AddScoped<IShowRepository, ShowsRepository>();
        }
    }
}