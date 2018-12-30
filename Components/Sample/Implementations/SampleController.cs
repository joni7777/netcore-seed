using System;
using Components.Sample.Interfaces;

namespace Components.Sample.Implementations
{
    public class SampleController:ISampleController
    {
        public string[] Get()
        {
            throw new ApplicationException();
            return Get(6);
        }

        public string[] Get(int id)
        {
            return new []{"Sample", "Sample2", id.ToString()};
        }
    }
}