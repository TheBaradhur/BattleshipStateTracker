namespace BattleshipStateTracker.Domain.Models
{
    public class GameState
    {
        public Player Player1 { get; set; }

        public GameStatus Status { get; set; } = GameStatus.NoGame;
    }
}