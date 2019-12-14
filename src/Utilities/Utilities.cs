namespace pacman.Utilities
{
    public struct Position_t
    {
        public int Column { get; set; }
        public int Row { get; set; }

        public Position_t(int c, int r)
        {
            Column = c;
            Row = r;
        }
    }

    public struct Coords_t
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Coords_t(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    public enum TileContent
    {
        none = 0,
        dot,
        superDot,
        wall,
        ghosthouse,
        ghosthouseDoor,
        tunel
    }
    public enum Direction
    {
        none = 0,
        up,
        down,
        left,
        right,
        nOfDirections
    }
}
