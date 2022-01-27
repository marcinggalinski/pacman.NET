namespace pacman.NET.Types;

public enum Direction
{
    None,
    Left,
    Up,
    Right,
    Down
}

public enum TileType
{
    Accessible,
    Inaccessible,
    Wall,
    GhostHouseDoor
}

public enum TileContent
{
    None,
    Dot,
    SuperDot
}

public enum EqualityType
{
    RowColumnBased,
    XYBased
}
