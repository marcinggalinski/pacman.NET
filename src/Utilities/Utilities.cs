namespace Pacman.Utilities
{
    public struct Resolution_t
    {
        public uint Width { get; set; }
        public uint Height { get; set; }
    }

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
        None = 0,
        Dot,
        SuperDot,
        Wall,
        Ghosthouse,
        GhosthouseDoor,
        Tunel
    }
    public enum Direction
    {
        None = 0,
        Up,
        Down,
        Left,
        Right,
        NOfDirections
    }
}
