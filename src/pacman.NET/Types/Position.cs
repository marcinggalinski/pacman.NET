using pacman.NET.Config;

namespace pacman.NET.Types;

public struct Position
{
    public uint Column { get; set; }
    public uint Row { get; set; }

    public float X => (Column - 1) * Constants.TileSize + Constants.TileSize / 2;
    public float Y => (Row - 1) * Constants.TileSize + Constants.TileSize / 2;


    public Position() : this(0, 0)
    { }

    public Position(uint column, uint row)
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
