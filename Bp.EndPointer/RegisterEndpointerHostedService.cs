using System;
using System.Threading;
using System.Threading.Tasks;
using Bp.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Bp.EndPointer
{
	public class RegisterEndpointerHostedService : IHostedService, IDisposable
	{
		private string _serverAddress;
		private EndPointer _endPointer;

		public RegisterEndpointerHostedService(IHostingEnvironment hostingEnvironment, IConfiguration config,
			ILogger<EndPointer> logger)
		{
			var serviceInfo = config.GetSection("Service").Get<ServiceInfo>();
			var endPointerInfo = config.GetSection("EndPointer").Get<EndPointerInfo>();
			_endPointer = new EndPointer(endPointerInfo.Url, hostingEnvironment.EnvironmentName, serviceInfo, logger);
			_serverAddress = ParseServerAddress(config.GetSection("Kestrel:Endpoints:Http:Url").Value ??
			                                    config.GetSection("Kestrel:Endpoints:Https:Url").Value);
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			return RegisterToEndPointer();
		}

		private async Task RegisterToEndPointer()
		{
			await _endPointer.Register(_serverAddress);
		}

		private string ParseServerAddress(string address)
		{
			return address
				.Replace("[::]", Environment.UserDomainName)
				.Replace("localhost", Environment.UserDomainName)
				.Replace("*", Environment.UserDomainName);
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}

		public void Dispose()
		{
			_endPointer.Dispose();
		}
	}
}