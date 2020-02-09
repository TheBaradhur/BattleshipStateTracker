using System.Collections.Generic;
using BattleshipStateTracker.Domain.Models.Ships;

namespace BattleshipStateTracker.Domain.Models
{
    public class Board
    {
        public int TotalNumberOfShips { get; set; }

        public int VerticalSize { get; set; }

        public int HorizontalSize { get; set; }

        public List<Ship> Ships { get; set; }

        public List<Coordinate> AttacksReceivedHistory { get; set; }

        public Board(int verticalSize, int horizontalSize, int totalNumberOfShips)
        {
            VerticalSize = verticalSize;
            HorizontalSize = horizontalSize;
            TotalNumberOfShips = totalNumberOfShips;

            Ships = new List<Ship>();
        }
    }
}