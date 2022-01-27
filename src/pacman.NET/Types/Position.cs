using pacman.NET.Config;
using SFML.System;
using System;

namespace pacman.NET.Types;

public struct Position
{
    public int Column { get; set; }
    public int Row { get; set; }

    public float X => Globals.LeftMargin + Column * Globals.TileSize + Globals.TileSize / 2;
    public float Y => Globals.TopMargin + Row * Globals.TileSize + Globals.TileSize / 2;


    public Position() : this(0, 0)
    { }

    public Position(int column, int row)
    {
        Column = column;
        Row = row;
    }
    

    public bool Equals(Position other) => Row == other.Row && Column == other.Column;
    
    public override bool Equals(object? obj) => obj is Position other && Equals(other);
    
    public override int GetHashCode() => HashCode.Combine(Row, Column);
    

    public static bool operator ==(Position lhv, Position rhv) => lhv.Row == rhv.Row && lhv.Column == rhv.Column;

    public static bool operator !=(Position lhv, Position rhv) => lhv.Row != rhv.Row || lhv.Column != rhv.Column;

    
    public static implicit operator Vector2f(Position position) => new Vector2f(position.X, position.Y);
}
