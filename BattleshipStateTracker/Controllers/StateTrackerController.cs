using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BattleshipStateTracker.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StateTrackerController : ControllerBase
    {
        private readonly ILogger<StateTrackerController> _logger;

        public StateTrackerController(ILogger<StateTrackerController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("OK");
        }
    }
}