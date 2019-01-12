using System;
using System.Reflection;
using Bp.HealthChecks;
using Bp.Logging.EnrichWithRequestParams;
using Bp.RouterAliases;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Bp.ApiRunner
{
    public class Startup
    {
        private MicroserviceInfo _serviceInfo;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _serviceInfo = Configuration.GetSection("Service").Get<MicroserviceInfo>();
            if(!_serviceInfo.IsValid()) throw new ArgumentException(MicroserviceInfo.MICROSERVICE_MISSING_CONFIGURATION_MESSAGE);
        }

        public IConfiguration Configuration { get; }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc(options => options.Conventions.Add(
                    new RouteTokenTransformerConvention(new RouterParameterTransformer())))
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddApplicationPart(Assembly.GetEntryAssembly());

            services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc(_serviceInfo.Version, new OpenApiInfo {Title = _serviceInfo.Name, Version = _serviceInfo.Version});
                    c.DescribeAllEnumsAsStrings();
                    c.DescribeStringEnumsInCamelCase();
                });
            services.ConfigureBpHealthChecksServices(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (!env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseEnrichWithRequestParams();
            app.UseBpHealthChecks();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(_serviceInfo.SwaggerUrl, $"{_serviceInfo.Name} V{_serviceInfo.Version}");
                c.DisplayOperationId();
            });
        }
    }
}