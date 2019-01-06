using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bp.Isolate
{
    public class IsolateBuilder : IIsolateBuilder
    {
        private readonly Stack<Action<IServiceCollection>> _servicesActions;
        private readonly Stack<Action<IConfigurationBuilder>> _configurationActions;

        private IsolateBuilder()
        {
            _servicesActions = new Stack<Action<IServiceCollection>>();
            _configurationActions = new Stack<Action<IConfigurationBuilder>>();
        }

        public IIsolateBuilder ConfigureServices(Action<IServiceCollection> configure)
        {
            _servicesActions.Push(configure);
            return this;
        }

        public IIsolateBuilder ConfigureConfiguration(Action<IConfigurationBuilder> configure)
        {
            _configurationActions.Push(configure);
            return this;
        }

        public IsolateAction Build()
        {
            var services = new ServiceCollection();
            foreach (var configure in _servicesActions)
            {
                configure(services);
            }
            var configurationBuilder = new ConfigurationBuilder();
            foreach (var configure in _configurationActions)
            {
                configure(configurationBuilder);
            }
            var configuration = configurationBuilder.Build();
            services.AddSingleton<IConfiguration>(configuration);

            var provider = services.BuildServiceProvider();

            return provider.GetRequiredService<IsolateAction>();
        }

        public static IIsolateBuilder For<TAction>(params string[] args) where TAction : IsolateAction
        {
            var environment = args[0];
            var data = new Dictionary<string, string> { ["environment"] = environment };
            var hostEnvironment = new HostingEnvironment
            {
                EnvironmentName = environment,
                ContentRootPath = Path.GetFullPath(Path.Combine("..", typeof(TAction).Assembly.GetName().Name.Replace(".Isolate", string.Empty)))
            };            
            return new IsolateBuilder()
                .ConfigureServices(services => 
                {
                    services.AddSingleton<IsolateAction, TAction>();
                    services.AddSingleton<IHostingEnvironment>(hostEnvironment);
                })
                .ConfigureConfiguration(config => config
                    .SetBasePath(hostEnvironment.ContentRootPath)
                    .AddInMemoryCollection(data)
                    .AddJsonFile("config/appsettings.json", optional: false));
        }
    }
}
