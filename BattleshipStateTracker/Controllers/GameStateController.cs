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
        public IActionResult NewGame([FromBody] NewGameRequest newGameRequest)
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

        [HttpPost("add-ship")]
        public IActionResult AddShip([FromBody] AddShipRequest addShipRequest)
        {
            try
            {
                var validation = _gameStateService.ValidateAddShipRequest(
                    addShipRequest.XPosition,
                    addShipRequest.YPosition,
                    addShipRequest.Orientation,
                    addShipRequest.Size,
                    addShipRequest.AddToPlayerOne);

                if (!validation.IsValid)
                {
                    return BadRequest(
                        ApiErrorResponse.GetCustomBadRequest(
                            "One or more game rules were not respected.",
                            HttpContext.TraceIdentifier,
                            new List<string> { validation.Error }));
                }

                _gameStateService.AddShipOnPlayersBoard(addShipRequest.XPosition,
                    addShipRequest.YPosition,
                    addShipRequest.Orientation,
                    addShipRequest.Size,
                    addShipRequest.AddToPlayerOne);

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

        [HttpPost("attack")]
        public IActionResult Attack([FromBody] AttackRequest attackRequest)
        {
            try
            {
                var validation = _gameStateService.ValidateAttackPosition(
                    attackRequest.XAttackCoordinate,
                    attackRequest.YAttackCoordinate,
                    attackRequest.TargetedUser);

                if (!validation.IsValid)
                {
                    return BadRequest(
                        ApiErrorResponse.GetCustomBadRequest(
                            "One or more game rules were not respected.",
                            HttpContext.TraceIdentifier,
                            new List<string> { validation.Error }));
                }

                var attackOutcome = _gameStateService.AttackPlayersPosition(
                    attackRequest.XAttackCoordinate,
                    attackRequest.YAttackCoordinate,
                    attackRequest.TargetedUser);

                return Ok(attackOutcome);
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
    }
}