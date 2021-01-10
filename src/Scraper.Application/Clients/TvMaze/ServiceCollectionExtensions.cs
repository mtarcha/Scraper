using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;


namespace Scraper.Application.Clients.TvMaze
{
    public static class ServiceCollectionExtensions
    {
        public static void AddTvMazeClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TvMazeApiSettings>(
                configuration.GetSection(nameof(TvMazeApiSettings)));
            services.AddHttpClient<ITvMazeApiClient, TvMazeApiClient>((serviceProvider, httpClient) =>
            {
                var options = serviceProvider.GetService<IOptions<TvMazeApiSettings>>().Value;
                httpClient.BaseAddress = new Uri(options.BaseUrl);
            });
        }
    }
}