using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using QuickAnswer.Database;

namespace QuickAnswer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<SQLiteConnection>>();

                var connection = new SQLiteConnection("Data Source=:memory:");
                logger.LogInformation($"New connection is created.");

                connection.Disposed += (sender, args)
                    => logger.LogInformation("Connection is disposed.");

                return connection;
            });
            services.AddTransient<Repository>();
            services.AddMvc();
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI();
            
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapSwagger();
                endpoints.MapControllers();
            });
        }
    }
}