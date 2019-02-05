using System;
using System.Linq;
using System.Threading.Tasks;
using Bp.Common;
using Bp.Config;
using Bp.EndPointer;
using Bp.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;

namespace Bp.ApiRunner
{
	public class Runner
	{
		public static void Run()
		{
			new WebHostBuilder()
				.ConfigureAppConfiguration(ConfigureConfiguration.AddConfigurationByEnvironment)
				.UseKestrel((builderContext, options) =>
				{
					options.Configure(builderContext.Configuration.GetSection("Kestrel"));
				})
				.UseSerilog(SerilogInit.ConfigureLogger)
				.UseStartup<Startup>()
				.Build()
				.Run();
		}
	}
}