using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BpSeed.Isolate
{
    public interface IIsolateBuilder
    {
        IIsolateBuilder ConfigureServices(Action<IServiceCollection> configure);
        IIsolateBuilder ConfigureConfiguration(Action<IConfigurationBuilder> configure);
        IsolateAction Build();
    }

    public interface IConfigureAction
    {
        IConfigureEnvironment UseAction<TAction>() where TAction : IsolateAction;
    }
    public interface IConfigureEnvironment
    {
        IIsolateBuilder UseEnvironment(string environment);
    }

    public static class IsolateBuilderExtensions
    {
        public static IIsolateBuilder UseAction<TAction>(this IIsolateBuilder builder) where TAction : IsolateAction
        {            
            return builder;
        }

        public static IIsolateBuilder UseEnvironment(this IIsolateBuilder builder, string environment)
        {
            var data = new Dictionary<string, string> { ["environment"] = environment };
            builder.ConfigureConfiguration(config => config.AddInMemoryCollection(data));
            var hostEnvironment = new HostingEnvironment
            {
                EnvironmentName = environment,                
            };
            builder.ConfigureServices(services => services.AddSingleton(hostEnvironment));
            return builder;
        }
    }
}
