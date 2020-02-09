using System.Collections.Generic;
using BattleshipStateTracker.Domain.Models;
using BattleshipStateTracker.Domain.Models.Ships;

namespace BattleshipStateTracker.Domain.Helpers
{
    internal static class ShipHelper
    {
        internal static bool IsCoordinateOverlappingAnotherShip(int x, int y, List<Ship> ships)
        {
            var coordinate = new Coordinate(x, y);

            foreach (var ship in ships)
            {
                if (ship.ShipCoordinates.ContainsKey(coordinate))
                {
                    return true;
                }
            }

            return false;
        }
    }
}