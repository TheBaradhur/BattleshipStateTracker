using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleshipStateTracker.Domain.Models.Ships
{
    public class Ship
    {
        public int XPosition { get; set; }

        public int YPosition { get; set; }

        public Orientation ShipOrientation { get; set; }

        public int Size { get; set; }

        public bool IsSunk => ShipCoordinates.Values.All(value => value != ShipCellState.Alive);

        public Dictionary<Coordinate, ShipCellState> ShipCoordinates { get; set; }

        public Ship(int xPosition, int yPosition, Orientation shipOrientation, int size)
        {
            XPosition = xPosition;
            YPosition = yPosition;
            ShipOrientation = shipOrientation;
            Size = size;

            ShipCoordinates = GenerateNewShipCoordinates();
        }

        private Dictionary<Coordinate, ShipCellState> GenerateNewShipCoordinates()
        {
            ShipCoordinates = new Dictionary<Coordinate, ShipCellState>();
            var initialCoord = new Coordinate(XPosition, YPosition);

            ShipCoordinates.Add(initialCoord, ShipCellState.Alive);

            var xOtherEnd = XPosition;
            var yOtherend = YPosition;

            for (int i = 1; i < Size; i++)
            {
                if (ShipOrientation == Orientation.Down)
                {
                    xOtherEnd += 1;
                }
                if (ShipOrientation == Orientation.Up)
                {
                    xOtherEnd -= 1;
                }

                if (ShipOrientation == Orientation.Left)
                {
                    yOtherend -= 1;
                }
                if (ShipOrientation == Orientation.Right)
                {
                    yOtherend += 1;
                }

                var rollingCoordinate = new Coordinate(xOtherEnd, yOtherend);
                ShipCoordinates.Add(rollingCoordinate, ShipCellState.Alive);
            }

            return ShipCoordinates;
        }
    }
}