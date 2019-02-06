﻿using System.Reflection;
using Bp.Common;
using Bp.EndPointer;
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
        private ServiceInfo _service;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _service = Configuration.GetSection("Service").Get<ServiceInfo>();
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
                c.SwaggerDoc(_service.Version, new OpenApiInfo {Title = _service.Name, Version = _service.Version});
                c.DescribeAllEnumsAsStrings();
                c.DescribeStringEnumsInCamelCase();
            });
            services.AddCors(options => options.AddPolicy("AllowAll",
                policyBuilder => policyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
            services.ConfigureBpHealthChecksServices(Configuration);
            services.AddHostedService<RegisterEndpointerHostedService>();
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
                c.SwaggerEndpoint(_service.SwaggerUrl, $"{_service.Name} V{_service.Version}");
                c.DisplayOperationId();
            });
        }
    }
}