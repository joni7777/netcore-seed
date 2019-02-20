using Bp.RouterAliases;
using Components.Sample.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace Components.Sample.Implementations
{
    public class SampleRouter : RouterBase
    {
        private readonly ILogger _logger;
        private readonly ISampleController _sampleController;

        public SampleRouter(ILogger<SampleRouter> logger, ISampleController sampleController)
        {
            _logger = logger;
            _sampleController = sampleController;
        }

        [HttpGet]
        [SwaggerResponse(200, "OK")]
        public string[] Get()
        {
            _logger.LogInformation("6");
            _logger.LogInformation("8");
            _logger.LogInformation("7");
            return _sampleController.Get();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [SwaggerResponse(200, "Sample", typeof(string[]))]
        public string[] GetById([BindRequired] int id)
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