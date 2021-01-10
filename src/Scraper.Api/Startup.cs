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
using Swashbuckle.AspNetCore.Swagger;

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
            services.AddMediatR(allAssemblies);

            services
                .AddSwaggerGen(c =>
                {
                    c.DescribeAllEnumsAsStrings();

                    c.SwaggerDoc("v1", new Info
                    {
                        Version = "v1",
                        Title = "Scraper Api",
                        Contact = new Contact
                        {
                            Name = "Scraper",
                            Email = "mtarcha@outlook.com",
                            Url = "https://github.com/mtarcha/Scraper"
                        }
                    });
                });
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
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Scraper V1");
            });
        }
    }
}
