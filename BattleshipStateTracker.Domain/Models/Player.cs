using System;
using System.Linq;

namespace BattleshipStateTracker.Domain.Models
{
    public class Player
    {
        public string Name { get; set; }

        public bool IsPlayerOne { get; set; }

        public Board PlayerBoard { get; set; }

        public bool AreAllShipsPlaced => PlayerBoard.Ships.Count == PlayerBoard.TotalNumberOfShips;

        public bool AreAllShipsSunk => PlayerBoard.Ships.All(x => x.IsSunk);

        public Player(string playerName)
        {
            Name = playerName;
        }
    }
}