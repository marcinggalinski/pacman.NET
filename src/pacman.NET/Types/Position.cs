using pacman.NET.Config;
using System;

namespace pacman.NET.Types;

public struct Position
{
    public int Column { get; set; }
    public int Row { get; set; }

    public float X => (Column - 1) * Constants.TileSize + Constants.TileSize / 2;
    public float Y => (Row - 1) * Constants.TileSize + Constants.TileSize / 2;


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
    

    public static bool operator ==(Position lhv, Position rhv)
    {
        return lhv.Row == rhv.Row && lhv.Column == rhv.Column;
    }

    public static bool operator !=(Position lhv, Position rhv)
    {
        return lhv.Row != rhv.Row || lhv.Column != rhv.Column;
    }
}
