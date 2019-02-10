using Components.Sample.Implementations;
using Components.Sample.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Components
{
	// Static class with no usage, Loaded with reflection by Bp.ExtendConfigureServices
	// Used to extend the app service collection
	public class BpConfigureServices
	{
		public static void ExtendConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<ISampleController, SampleController>();
		}
	}
}