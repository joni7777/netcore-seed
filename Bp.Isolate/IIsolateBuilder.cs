using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bp.Isolate
{
    public interface IIsolateBuilder
    {
        IIsolateBuilder ConfigureServices(Action<IServiceCollection> configure);
        IIsolateBuilder ConfigureConfiguration(Action<IConfigurationBuilder> configure);
        IsolateAction Build();
    }
}
