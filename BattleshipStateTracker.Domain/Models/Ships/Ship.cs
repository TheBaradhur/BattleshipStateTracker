using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleshipStateTracker.Domain.Models.Ships
{
    public class Ship
    {
        public int XTipPosition { get; set; }

        public int YTipPosition { get; set; }

        public Orientation Orientation { get; set; }

        public int Size { get; set; }

        public bool IsSunk => ShipCoordinates.Values.All(value => value != ShipCellState.Alive);

        public Dictionary<Coordinate, ShipCellState> ShipCoordinates { get; set; }

        public Ship(int xTipPosition, int yTipPosition, Orientation orientation, int size)
        {
            XTipPosition = xTipPosition;
            YTipPosition = yTipPosition;
            Orientation = orientation;
            Size = size;

            ShipCoordinates = GenerateNewShipCoordinates();
        }

        private Dictionary<Coordinate, ShipCellState> GenerateNewShipCoordinates()
        {
            throw new NotImplementedException();
        }
    }
}