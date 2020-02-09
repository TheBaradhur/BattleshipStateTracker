using BattleshipStateTracker.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BattleshipStateTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameStateController : ControllerBase
    {
        private readonly ILogger<GameStateController> _logger;

        private readonly IGameStateService _gameStateService;

        public GameStateController(ILogger<GameStateController> logger, IGameStateService gameStateService)
        {
            _logger = logger;
            _gameStateService = gameStateService;
        }
        
        [HttpGet("")]
        [HttpGet("/")]
        public IActionResult GameState()
        {
            return Ok(_gameStateService.GetGameState());
        }

        [HttpPost]
        public IActionResult CreateNewBoard()
        {
            return Ok("OK");
        }

        [HttpPost]
        public IActionResult AddBattleship()
        {
            return Ok("OK");
        }

        [HttpPost]
        public IActionResult AttackPosition()
        {
            return Ok("OK");
        }
    }
}