using BattleshipStateTracker.Domain.Models;

namespace BattleshipStateTracker.Domain
{
    public interface IGameStateService
    {
        void CreateNewBoard(string player1Name, int totalNumberOfShips);

        string GetGameState();
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

        public string GetGameState()
        {
            return _gameState.Type.ToString();
        }

        public void CreateNewBoard(string player1Name, int totalNumberOfShips)
        {
            _gameState.Player1 = new Player(player1Name)
            {
                PlayerBoard = new Board(VerticalBoardSize, HorizontalBoardSize, totalNumberOfShips)
            };
        }
    }
}