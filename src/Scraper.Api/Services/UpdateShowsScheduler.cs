using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Scraper.Application.Commands;

namespace Scraper.Api.Services
{
    public class UpdateShowsScheduler : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<UpdateShowsScheduler> _logger;

        public UpdateShowsScheduler(IServiceProvider serviceProvider, ILogger<UpdateShowsScheduler> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Update shows scheduler running.");

            while (true)
            {
                _logger.LogInformation("New update started");

                using (var scope = _serviceProvider.CreateScope())
                {
                    var mediator = scope.ServiceProvider.GetService<IMediator>();
                    await mediator.Send(new AddNewShowsCommand(), stoppingToken);
                }

                _logger.LogInformation("Update finished");
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Update shows scheduler is stopping.");

            return Task.CompletedTask;
        }
    }
}