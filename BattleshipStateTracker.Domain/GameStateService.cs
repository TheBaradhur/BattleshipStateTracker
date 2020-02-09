using BattleshipStateTracker.Domain.Models;

namespace BattleshipStateTracker.Domain
{
    public interface IGameStateService
    {
        void InitializeNewGame(string player1Name, int totalNumberOfShips);

        string GetGameStatus();

        string GetPlayerOneName();
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
            _gameState.Player1 = new Player(player1Name)
            {
                PlayerBoard = new Board(VerticalBoardSize, HorizontalBoardSize, totalNumberOfShips)
            };

            _gameState.Status = GameStatus.Initiated;
        }

        public string GetPlayerOneName()
        {
            return _gameState.Player1?.Name;
        }
    }
}