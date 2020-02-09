using BattleshipStateTracker.Domain.Models;
using BattleshipStateTracker.Domain.Models.Ships;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleshipStateTracker.Domain
{
    public partial class GameStateService : IGameStateService
    {
        private const int VerticalBoardSize = 10;

        private const int HorizontalBoardSize = 10;

        private const string MissText = "Attack missed...";

        private const string HitText = "Attack hit!!!";

        private string EndGameText(string playerName) => $"All ships of {playerName} are destroyed! Game over.";

        private readonly GameState _gameState;

        public GameStateService()
        {
            _gameState = new GameState();
        }

        public string GetGameStatus()
        {
            return _gameState.Status.ToString();
        }

        public void InitializeNewGame(string playerOneName, int totalNumberOfShips)
        {
            _gameState.PlayerOne = new Player(playerOneName)
            {
                PlayerBoard = new Board(VerticalBoardSize, HorizontalBoardSize, totalNumberOfShips),
                IsPlayerOne = true
            };

            _gameState.Status = GameStatus.Initiated;
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

        public string AttackPlayersPosition(int x, int y, int targetedPlayer)
        {
            var attackCoordinates = new Coordinate(x, y);

            var player = targetedPlayer == 1 ? _gameState.PlayerOne : null;

            var IsAttackSuccess = false;

            foreach (var ship in player.PlayerBoard.Ships)
            {
                var isShipShot = ship.ShipCoordinates.Keys.Contains(attackCoordinates);

                if (isShipShot)
                {
                    ship.ShipCoordinates[attackCoordinates] = ShipCellState.Shot;
                    IsAttackSuccess = true;

                    break;
                }
            }

            player.PlayerBoard.AttacksReceivedHistory.Add(attackCoordinates);

            if (_gameState.Status == GameStatus.ShipSetupCompleted)
            {
                _gameState.Status = GameStatus.OnGoing;
            }

            if (player.AreAllShipsSunk)
            {
                _gameState.Status = GameStatus.Finished;

                return EndGameText(player.Name);
            }

            return IsAttackSuccess ? HitText : MissText;
        }

        private static bool AreCoordinatesOverlappingAnotherShip(int x, int y, List<Ship> ships)
        {
            var coordinate = new Coordinate(x, y);

            foreach (var ship in ships)
            {
                if (ship.ShipCoordinates.ContainsKey(coordinate))
                {
                    return true;
                }
            }

            return false;
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