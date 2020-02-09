using BattleshipStateTracker.Domain.Models;

namespace BattleshipStateTracker.Domain
{
    public partial class GameStateService
    {
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

            if (AreCoordinatesOverlappingAnotherShip(
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

        public GameRulesValidation ValidateAttackPosition(int x, int y, int targetedPlayerNumber)
        {
            var targetedPlayer = targetedPlayerNumber == 1 ? _gameState.PlayerOne : null;

            if (_gameState.Status == GameStatus.NoGame)
            {
                return new GameRulesValidation(false, "GameNotInitialized",
                    "You need to initialize a game before adding a ship.");
            }

            if (_gameState.Status == GameStatus.ShipSetup)
            {
                return new GameRulesValidation(false, "ShipsSetupIncomplete",
                    $"You need to add all the ships for a user to be able to start the game, missing {targetedPlayer.PlayerBoard.TotalNumberOfShips - targetedPlayer.PlayerBoard.Ships.Count}.");
            }

            if (_gameState.Status == GameStatus.Finished)
            {
                return new GameRulesValidation(false, "GameFinished",
                    "The game is over, you cannot perform an attack. Please reinitialize the board to start a new game.");
            }

            if (!IsValidCoordinates(x, y))
            {
                return new GameRulesValidation(
                    false,
                    "InvalidAttackPosition",
                    $"You cannot attack outside the board boundaries. Should be between 1 and {VerticalBoardSize} for X, 1 and {HorizontalBoardSize} for Y.");
            }

            var coordinates = new Coordinate(x, y);

            if (targetedPlayerNumber == 1 && _gameState.PlayerOne.PlayerBoard.AttacksReceivedHistory.Contains(coordinates))
            {
                return new GameRulesValidation(
                    false,
                    "PositionAlreadyTargeted",
                    $"This position was already targeted, please pick another one.");
            }

            return new GameRulesValidation(true);
        }
    }
}