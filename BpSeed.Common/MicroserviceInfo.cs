using System;

namespace BpSeed.Common
{
    public class MicroserviceInfo
    {
        public string Name { get; set; }
        public string Version { get; set; }
        
        public bool IsValid() =>
            !String.IsNullOrEmpty(Name) && !String.IsNullOrEmpty(Version);

        public const string MICROSERVICE_MISSING_CONFIGURATION_MESSAGE = "Microservice missing required configuration";

        public MicroserviceInfo(){}

        public MicroserviceInfo(string name, string version)
        {
            Name = name;
            Version = version;
        }

        public override string ToString()
        {
            return $"{Name}-{Version}";
        }
    }
}