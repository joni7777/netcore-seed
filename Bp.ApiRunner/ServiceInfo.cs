namespace BpSeed.API
{
    public class ServiceInfo
    {
        public string Name { get; set; }
        public string Version { get; set; }

        public ServiceInfo()
        {
        }

        public ServiceInfo(string name, string version)
        {
            Name = name;
            Version = version;
        }
    }
}