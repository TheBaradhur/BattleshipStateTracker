using System;

namespace BattleshipStateTracker.Domain.Models
{
    public class Player
    {
        public string Name { get; set; }

        public Board PlayerBoard { get; set; }

        public bool AreAllShipsPlaced => PlayerBoard.Ships.Count == PlayerBoard.TotalNumberOfShips;

        public Player(string playerName)
        {
            Name = playerName;
        }
    }
}