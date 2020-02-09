namespace BattleshipStateTracker.Domain.Models
{
    public class Coordinate
    {
        public int XCoordinate { get; set; }

        public int YCoordinate { get; set; }

        public Coordinate(int x, int y)
        {
            XCoordinate = x;
            YCoordinate = y;
        }
    }
}