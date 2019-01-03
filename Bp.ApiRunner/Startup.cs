using System.Reflection;
using Bp.RouterAliases;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace BpSeed.API
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
            services.AddDbContext<SchoolContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            
            services
                .AddMvc(options => options.Conventions.Add(
                    new RouteTokenTransformerConvention(new RouterParameterTransformer())))
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                    .AddApplicationPart(Assembly.GetEntryAssembly());
            
            services.AddSwaggerGen(c =>
            {
                
                c.SwaggerDoc(_service.Version, new OpenApiInfo {Title = _service.Name, Version = _service.Version});
            });
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
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint($"/swagger/{_service.Version}/swagger.json", $"{_service.Name} V{_service.Version}"); });
        }
    }

    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {
        }
    }
}