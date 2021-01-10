using System;
using System.Linq;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scraper.Api.Services;
using Scraper.Application.Clients.TvMaze;
using Scraper.Application.Commands;
using Scraper.Infrastructure.Mongo;

namespace Scraper.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<UpdateShowsScheduler>();
            services.AddMongoDbRepository(Configuration);
            services.AddTvMazeClient(Configuration);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            services.AddMediatR(typeof(AddNewShowsCommandHandler));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
