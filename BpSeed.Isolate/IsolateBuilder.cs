using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BpSeed.Isolate
{
    public class IsolateBuilder : IIsolateBuilder, IConfigureAction, IConfigureEnvironment
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

        public IConfigureEnvironment UseAction<TAction>() where TAction : IsolateAction
        {
            ConfigureServices(services => services.AddSingleton<IsolateAction, TAction>());
            return this;
        }

        public IIsolateBuilder UseEnvironment(string environment)
        {
            var data = new Dictionary<string, string> { ["environment"] = environment };
            ConfigureConfiguration(config => config.AddInMemoryCollection(data));
            var hostEnvironment = new HostingEnvironment
            {
                EnvironmentName = environment,
            };
            ConfigureServices(services => services.AddSingleton(hostEnvironment));
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
                ContentRootPath = Path.Combine("..", typeof(TAction).Assembly.GetName().Name.Replace(".Isolate", string.Empty))
            };
            return new IsolateBuilder()
                .ConfigureServices(services => 
                {
                    services.AddSingleton<IsolateAction, TAction>();
                    services.AddSingleton<IHostingEnvironment>(hostEnvironment);
                })
                .ConfigureConfiguration(config => config.AddInMemoryCollection(data));
        }
    }
}
