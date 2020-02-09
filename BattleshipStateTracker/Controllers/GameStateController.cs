using BattleshipStateTracker.Api.Models;
using BattleshipStateTracker.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

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
            try
            {
                var status = _gameStateService.GetGameStatus();

                _logger.LogDebug($"Game Status: {status}");

                return Ok(status);
            }
            catch (Exception e)
            {
                return StatusCode(500,
                    ApiErrorResponse.GetCustomInternalServerError(
                        "An unexpected error occured. Please contact API team.",
                        HttpContext.TraceIdentifier,
                        new List<string> { e.Message }));
            }
        }

        [HttpPost("new-game")]
        public IActionResult NewGame(NewGameRequest newGameRequest)
        {
            try
            {
                _gameStateService.InitializeNewGame(newGameRequest.PlayerOneName, newGameRequest.TotalNumberOfShipsPerPlayer);

                _logger.LogDebug("New Game initiated");

                return StatusCode(201, _gameStateService.GetGameStatus());
            }
            catch (Exception e)
            {
                return StatusCode(500,
                    ApiErrorResponse.GetCustomInternalServerError(
                        "An unexpected error occured. Please contact API team.",
                        HttpContext.TraceIdentifier,
                        new List<string> { e.Message }));
            }
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