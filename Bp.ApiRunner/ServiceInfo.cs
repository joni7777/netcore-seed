namespace Bp.ApiRunner
{
    public class ServiceInfo
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string SwaggerUrl => $"/swagger/{Version}/swagger.json";

        public ServiceInfo()
        {
        }
    }
}