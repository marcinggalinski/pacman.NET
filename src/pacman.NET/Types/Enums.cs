namespace pacman.NET.Types;

public enum MoveDirection
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
