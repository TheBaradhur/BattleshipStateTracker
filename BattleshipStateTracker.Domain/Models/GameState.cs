namespace BattleshipStateTracker.Domain.Models
{
    public class GameState
    {
        public Player PlayerOne { get; set; }

        public GameStatus Status { get; set; } = GameStatus.NoGame;
    }
}