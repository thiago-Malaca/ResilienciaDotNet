using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;

using Resiliente.ServicoA.Commands;
using Resiliente.ServicoA.Services;

using Steeltoe.CircuitBreaker.Hystrix;
using Steeltoe.Discovery.Client;
using Steeltoe.Management.Endpoint.Env;
// using Steeltoe.Management.CloudFoundry;
using Steeltoe.Management.Endpoint.Health;
using Steeltoe.Management.Endpoint.Info;
using Steeltoe.Management.Endpoint.Loggers;
using Steeltoe.Management.Endpoint.Mappings;
using Steeltoe.Management.Endpoint.Metrics;
using Steeltoe.Management.Endpoint.Refresh;
using Steeltoe.Management.Endpoint.Trace;

namespace Resiliente.ServicoA
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
            services.AddFeatureManagement();

            services.AddHystrixCommand<BacenCommand>("BacenCommand", Configuration);

            services.AddDiscoveryClient(Configuration);

            services.AddSingleton<IBacenService, BacenService>();

            services.AddHealthActuator(Configuration);
            services.AddInfoActuator(Configuration);
            services.AddTraceActuator(Configuration);
            services.AddRefreshActuator(Configuration);
            services.AddEnvActuator(Configuration);
            services.AddMappingsActuator(Configuration);
            services.AddMetricsActuator(Configuration);

            services.AddControllers();

            services.AddHystrixMetricsStream(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.MapWhen(context => context.Request.Method.Equals("options", StringComparison.OrdinalIgnoreCase), HandleHead);
            app.UseCors(options => options.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());

            app.UseHealthActuator();
            app.UseInfoActuator();
            app.UseTraceActuator();
            app.UseRefreshActuator();
            app.UseEnvActuator();
            app.UseMappingsActuator();
            app.UseMetricsActuator();

            app.UseDiscoveryClient();
            app.UseHystrixRequestContext();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseHystrixMetricsStream();
        }
        private static void HandleHead(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                context.Response.StatusCode = 200;
                await context.Response.WriteAsync($"Up to head!");
            });
        }

    }
}