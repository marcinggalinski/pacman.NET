using Pacman.Entities;
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

        public static bool operator==(Position_t lhv, Position_t rhv)
        {
            return lhv.Column == rhv.Column && lhv.Row == rhv.Row;
        }
        public static bool operator!=(Position_t lhv, Position_t rhv)
        {
            return lhv.Column != rhv.Column || lhv.Row != rhv.Row;
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
        public static Position_t operator-(Position_t lhv, Position_t rhv)
        {
            return new Position_t(lhv.Column - rhv.Column, lhv.Row - rhv.Row);
        }
        public static Position_t operator%(Position_t lhv, Vector2i rhv)
        {
            return new Position_t(lhv.Column % rhv.X, lhv.Row % rhv.Y);
        }

        public static implicit operator Position_t(Vector2f v)
        {
            return new Position_t((int)v.X, (int)v.Y);
        }
        public static implicit operator Vector2i(Position_t pos)
        {
            return new Vector2i(pos.Column, pos.Row);
        }

        public override string ToString()
        {
            return $"({Column} {Row})";
        }

        public override bool Equals(object obj)
        {
            return obj is Position_t t &&
                   Column == t.Column &&
                   Row == t.Row;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Column, Row);
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

    public struct DotCounter
    {
        public int Count { get; private set; }

        public void Restart() => Count = 0;
        public void Add()
        {
            Count = Count + 1;
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
    public enum GhostMode
    {
        Idle = 0,
        Scatter,
        Chase,
        Frightened,
        Dead
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

    public static class Functions
    {
        public static int Distance(Position_t p1, Position_t p2)
        {
            return (int)Math.Sqrt(Math.Pow(p2.Column - p1.Column, 2) + Math.Pow(p2.Row - p1.Row, 2));
        }

        public static double DirDot(Direction dir1, Direction dir2)
        {
            var v1 = Directions.Table[(int)dir1];
            var v2 = Directions.Table[(int)dir2];

            return v1.X * v2.X + v1.Y * v2.Y;
        }
        
        public static Tile[,] DeepClone(Tile[,] tiles)
        {
            int c = tiles.GetLength(0);
            int r = tiles.GetLength(1);
            Tile[,] tiles_ = new Tile[c, r];

            for(int i = 0; i < c; i++)
                for(int j = 0; j < r; j++)
                    tiles_[i, j] = new Tile(tiles[i, j]);

            return tiles_;
        }
    }
}
