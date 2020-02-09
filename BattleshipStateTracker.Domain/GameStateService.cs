using BattleshipStateTracker.Domain.Models;
using BattleshipStateTracker.Domain.Models.Ships;
using System;
using BattleshipStateTracker.Domain.Helpers;

namespace BattleshipStateTracker.Domain
{
    public interface IGameStateService
    {
        void InitializeNewGame(string player1Name, int totalNumberOfShips);

        string GetGameStatus();

        string GetPlayerOneName();

        GameRulesValidation ValidateAddShipRequest(int xTipPosition, int yTipPosition, string orientation, int size, bool addToPlayerOne);

        void AddShipOnPlayersBoard(int xPosition, int yPosition, string orientation, int size, bool addToPlayerOne);
    }

    public class GameStateService : IGameStateService
    {
        private const int VerticalBoardSize = 10;

        private const int HorizontalBoardSize = 10;

        private readonly GameState _gameState;

        public GameStateService()
        {
            _gameState = new GameState();
        }

        public string GetGameStatus()
        {
            return _gameState.Status.ToString();
        }

        public void InitializeNewGame(string player1Name, int totalNumberOfShips)
        {
            _gameState.PlayerOne = new Player(player1Name)
            {
                PlayerBoard = new Board(VerticalBoardSize, HorizontalBoardSize, totalNumberOfShips)
            };

            _gameState.Status = GameStatus.Initiated;
        }

        public string GetPlayerOneName()
        {
            return _gameState.PlayerOne?.Name;
        }

        public GameRulesValidation ValidateAddShipRequest(int xPosition, int yPosition, string orientation, int size, bool addToPlayerOne)
        {
            if (_gameState.Status == GameStatus.NoGame)
            {
                return new GameRulesValidation(false, "GameNotInitialized",
                    "You need to initialize a game before adding a ship.");
            }

            if (_gameState.Status != GameStatus.NoGame
                && _gameState.Status != GameStatus.ShipSetup
                && _gameState.Status != GameStatus.Initiated)
            {
                return new GameRulesValidation(false, "SetupIsFinished",
                    "You cannot add another ship once the game has started or all ship are placed.");
            }

            if (size < 2)
            {
                return new GameRulesValidation(
                    false,
                    "InvalidShipSize",
                    $"A ship cannot be smaller than 2 cells.");
            }

            if (size > VerticalBoardSize || size > HorizontalBoardSize)
            {
                return new GameRulesValidation(
                    false,
                    "InvalidShipSize",
                    $"The Size of the ship is bigger than board boundaries. Max Size is {VerticalBoardSize} for X and '{HorizontalBoardSize}' for Y.");
            }

            if (!IsValidCoordinates(xPosition, yPosition))
            {
                return new GameRulesValidation(
                    false,
                    "InvalidShipPosition",
                    $"You cannot place a ship outside of the board boundaries. Should be between 1 and {VerticalBoardSize} for X, 1 and {HorizontalBoardSize} for Y.");
            }

            if (!IsShipFittingOnBoard(xPosition, yPosition, orientation, size))
            {
                return new GameRulesValidation(
                    false,
                    "InvalidShipPosition",
                    $"The ship is not entirely placed in the board boundaries. Each point of the boat " +
                        $"Should be between 1 and {VerticalBoardSize} for X, 1 and '{HorizontalBoardSize}' for Y.");
            }

            if (ShipHelper.IsCoordinateOverlappingAnotherShip(
                xPosition,
                yPosition,
                addToPlayerOne ? _gameState.PlayerOne.PlayerBoard.Ships : null))
            {
                return new GameRulesValidation(
                    false,
                    "InvalidShipPosition",
                    $"The ship is overlapping with another ship.");
            }

            return new GameRulesValidation(true);
        }

        public void AddShipOnPlayersBoard(int xPosition, int yPosition, string orientation, int size, bool addToPlayerOne)
        {
            var shipOrientation = (Orientation)Enum.Parse(typeof(Orientation), orientation, true);
            var newShip = new Ship(xPosition, yPosition, shipOrientation, size);

            if (addToPlayerOne)
            {
                _gameState.PlayerOne.PlayerBoard.Ships.Add(newShip);
            }
            else
            {
                // Handle player 2 here    
            }
            
            if (_gameState.Status != GameStatus.ShipSetup)
            {
                _gameState.Status = GameStatus.ShipSetup;
            }
            
            if (addToPlayerOne && _gameState.PlayerOne.AreAllShipsPlaced)
            {
                _gameState.Status = GameStatus.ShipSetupCompleted;
            }
        }

        private bool IsShipFittingOnBoard(int xPosition, int yPosition, string orientation, int size)
        {
            var shipOrientation = (Orientation)Enum.Parse(typeof(Orientation), orientation, true);
            var xOtherEnd = xPosition;
            var yOtherEnd = yPosition;

            if (shipOrientation == Orientation.Down)
            {
                xOtherEnd += size;
            }

            if (shipOrientation == Orientation.Up)
            {
                xOtherEnd -= size;
            }

            if (shipOrientation == Orientation.Left)
            {
                yOtherEnd -= size;
            }

            if (shipOrientation == Orientation.Right)
            {
                yOtherEnd += size;
            }

            if (!IsValidCoordinates(xOtherEnd, yOtherEnd))
            {
                return false;
            }

            return true;
        }

        private bool IsValidCoordinates(int xOtherEnd, int yOtherEnd)
        {
            return xOtherEnd <= VerticalBoardSize
                   && yOtherEnd <= HorizontalBoardSize
                   && xOtherEnd >= 1
                   && yOtherEnd >= 1;
        }
    }
}