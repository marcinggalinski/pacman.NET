using pacman.NET.Config;
using SFML.System;
using System;

namespace pacman.NET.Types;

public struct Position
{
    public Position(uint column, uint row)
    {
        X = Globals.LeftMargin + column * Globals.TileSize + Globals.TileSize / 2;
        Y = Globals.TopMargin + row * Globals.TileSize + Globals.TileSize / 2;
    }

    public Position(float x, float y)
    {
        X = x;
        Y = y;
    }
    
    
    public float X { get; set; }
    public float Y { get; set; }

    public uint Column => (uint)Math.Round((X - Globals.LeftMargin - Globals.TileSize / 2) / Globals.TileSize);
    public uint Row => (uint)Math.Round((Y - Globals.TopMargin - Globals.TileSize / 2) / Globals.TileSize);


    public bool Equals(Position position, EqualityType type)
    {
        return type switch
        {
            EqualityType.RowColumnBased => Row == position.Row && Column == position.Column,
            EqualityType.XYBased => Math.Abs(X - position.X) < float.Epsilon && Math.Abs(Y - position.Y) < float.Epsilon,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }


    public static implicit operator Vector2f(Position position) => new Vector2f(position.X, position.Y);
}
