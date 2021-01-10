using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;


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
            }).AddPolicyHandler(GetRetryPolicy());
        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == (System.Net.HttpStatusCode)429)
                .WaitAndRetryAsync(7, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}