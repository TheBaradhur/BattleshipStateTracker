namespace BattleshipStateTracker.Domain.Models
{
    public class Player
    {
        public string Name { get; set; }

        public Board PlayerBoard { get; set; }

        public Player(string playerName)
        {
            Name = playerName;
        }
    }
}