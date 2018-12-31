using Components.Sample.Interfaces;

namespace Components.Sample.Implementations
{
    public class SampleControllerFactory : ISampleControllerFactory
    {
        public ISampleController Create()
        {
            return new SampleController();
        }
    }
}