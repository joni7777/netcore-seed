using Sample.Interfaces;

namespace Sample.Implementations
{
    public class SampleControllerFactory : ISampleControllerFactory
    {
        public ISampleController Create()
        {
            return new SampleController();
        }
    }
}