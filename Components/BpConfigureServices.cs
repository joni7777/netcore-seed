using Components.Sample.Implementations;
using Components.Sample.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Components
{
	// Static class with no usage, Loaded with reflection by Bp.ExtendConfigureServices
	// Used to extend the app service collection
	public class BpConfigureServices
	{
		public static void ExtendConfigureServices(IServiceCollection services, IConfiguration configuration)
		{
			services.AddSingleton<ISampleController, SampleController>();
		}
		
		public static void ExtendConfigure(IApplicationBuilder app, IHostingEnvironment env, IConfiguration configuration)
		{
			// Example for the usage:
			// app.UseMvc();
		}
	}
}