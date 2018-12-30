using System;
using Bp.RouterAliases;
using Components.Sample.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Components.Sample.Implementations
{
    public class SampleRouter : RouterBase
    {
        private readonly ISampleController _sampleController = new SampleControllerFactory().Create();
        private readonly ILogger _logger;

        public SampleRouter([FromServices] ILogger<SampleRouter> logger)
        {
            _logger = logger;
        }
        
        [HttpGet]
        public string[] Get()
        {
            _logger.LogInformation("6");
            return _sampleController.Get();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string[] Get(int id)
        {
            return _sampleController.Get(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}