using Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Consumer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConsumersController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<ConsumersController> _logger;

        public ConsumersController(ILogger<ConsumersController> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            _memoryCache = memoryCache;
        }

        [HttpGet("messages")]
        public ActionResult<IEnumerable<TestMessage>> Get()
        {
            if(_memoryCache.TryGetValue("messages", out object? value) && value is IEnumerable<TestMessage> messages)
            {
                return Ok(messages);
            }

            return Ok(new List<TestMessage>());
        }
    }
}
