using System;
using SFML.System;

namespace Pacman.Utilities
{
    public struct Resolution_t
    {
        public uint Width { get; set; }
        public uint Height { get; set; }

        public override string ToString()
        {
            return $"({Width} {Height})";
        }
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

        public static Position_t operator+(Position_t lhv, Position_t rhv)
        {
            return new Position_t(lhv.Column + rhv.Column, lhv.Row + rhv.Row);
        }
        public static Position_t operator+(Position_t? lhv, Position_t rhv)
        {
            if(lhv == null)
                return new Position_t(rhv.Column, rhv.Row);
            else
                return new Position_t(lhv.Value.Column + rhv.Column, lhv.Value.Row + rhv.Row);
        }
        public static Position_t operator%(Position_t lhv, Vector2i rhv)
        {
            return new Position_t(lhv.Column % rhv.X, lhv.Row % rhv.Y);
        }

        public static implicit operator Position_t(Vector2f v)
        {
            return new Position_t((int)v.X, (int)v.Y);
        }

        public override string ToString()
        {
            return $"({Column} {Row})";
        }
    }

    public struct Coords_t
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Coords_t(float x, float y)
        {
            X = x;
            Y = y;
        }
        public Coords_t(float val)
        {
            X = val;
            Y = val;
        }
        public Coords_t(Coords_t coords)
        {
            X = coords.X;
            Y = coords.Y;
        }

        public static bool operator==(Coords_t lhv, Coords_t rhv)
        {
            return lhv.X == rhv.X && lhv.Y == rhv.Y;
        }
        public static bool operator!=(Coords_t lhv, Coords_t rhv)
        {
            return lhv.X != rhv.X || lhv.Y != rhv.Y;
        }
        
        public static Coords_t operator+(Coords_t lhv, Coords_t rhv)
        {
            return new Coords_t(lhv.X + rhv.X, lhv.Y + rhv.Y);
        }
        public static Coords_t operator+(Coords_t lhv, Vector2f rhv)
        {
            return new Coords_t(lhv.X + rhv.X, lhv.Y + rhv.Y);
        }
        public static Coords_t operator-(Coords_t lhv, Coords_t rhv)
        {
            return new Coords_t(lhv.X - rhv.X, lhv.Y - rhv.Y);
        }
        public static Coords_t operator*(Coords_t lhv, float rhv)
        {
            return new Coords_t(lhv.X * rhv, lhv.Y * rhv);
        }
        
        public static implicit operator Coords_t(Vector2f v)
        {
            return new Coords_t(v.X, v.Y);
        }
        public static implicit operator Vector2f(Coords_t c)
        {
            return new Vector2f((float)c.X, (float)c.Y);
        }

        public override string ToString()
        {
            return $"({X} {Y})";
        }

        public override bool Equals(object obj)
        {
            return obj is Coords_t t &&
                   X == t.X &&
                   Y == t.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
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

    public static class Directions
    {
        public static Vector2f[] Table = {
            new Vector2f(0, 0),
            new Vector2f(0, -1),
            new Vector2f(0, 1),
            new Vector2f(-1, 0),
            new Vector2f(1, 0)
        };
    }

    public class InvalidTilePositionException : Exception
    {
        public InvalidTilePositionException(Position_t pos)
            : base("Invalid tile position: " + pos) {}
        public InvalidTilePositionException(object pos)
            : base("Invalid position type: " + pos.GetType()) {}
    }
}
