using System;

namespace Bp.ApiRunner
{
    public class MicroserviceInfo
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string SwaggerUrl => $"/swagger/{Version}/swagger.json";

        public bool IsValid() =>
            !String.IsNullOrEmpty(Name) && !String.IsNullOrEmpty(Version);

        public const string MICROSERVICE_MISSING_CONFIGURATION_MESSAGE = "Microservice missing required configuration";
        
        public MicroserviceInfo()
        {
        }
    }
}