using Balena.IOT.Entity.Entities;
using Balena.IOT.MessageQueue;
using Balena.IOT.RealTimeMonitor.Api.HostedService;
using Balena.IOT.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Balena.IOT.RealTimeMonitor.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Balena.IOT.RealTimeMonitor.Api", Version = "v1" });
            });
            
            //adds mock repository for the device and telemetry entity
            //using singleton because repository is in memory and while application is running
            //application state should be saved across the calls
            services.AddSingleton<IRepository<Device>, InMemoryRepository<Device>>();
            services.AddSingleton<IRepository<DeviceTelemetry>, InMemoryRepository<DeviceTelemetry>>();

            //add internal message broker for processing purposes
            services.AddSingleton<IMessageBroker, InternalMessageBroker>();

            //add hosted service to process telemetry
            services.AddHostedService<TelemetryProcessorHostedService>();

            //add fake data
            services.AddHostedService<FakeDataHostedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Balena.IOT.RealTimeMonitor.Api");
            });


            app.UseMvc();
        }
    }
}