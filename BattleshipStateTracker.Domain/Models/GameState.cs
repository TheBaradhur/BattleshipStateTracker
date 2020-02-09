namespace BattleshipStateTracker.Domain.Models
{
    public class GameState
    {
        public Player Player1 { get; set; }

        public GameStatus Type { get; set; } = GameStatus.NoGame;
    }
}