using pacman.NET.Config;
using pacman.NET.Types;
using SFML.Graphics;
using SFML.System;

namespace pacman.NET.Models;

public class Pacman : ModelBase
{
    public Direction MoveDirection { get; private set; } = Direction.None;
    public Direction PlannedTurn { get; private set; } = Direction.None;

    public Pacman(Position startPosition)
    {
        Position = startPosition;
        Sprite = new Sprite(Globals.Textures.Pacman)
        {
            Position = new Vector2f(Position.X - Globals.TileSize / 2, Position.Y - Globals.TileSize / 2),
            Scale = new Vector2f(Globals.TileSize / Globals.Textures.Pacman.Size.X, Globals.TileSize / Globals.Textures.Pacman.Size.Y)
        };
    }

    public void PlanTurn(Direction direction)
    {
        PlannedTurn = direction;
    }

    public void Move(Map map)
    {
        CheckTurnPossibility(map);
        if (MoveDirection is Direction.None)
            return;

        var positionChange = MoveDirection switch
        {
            Direction.Left => new Vector2f(-1f, 0f),
            Direction.Up => new Vector2f(0f, -1f),
            Direction.Right => new Vector2f(1f, 0f),
            Direction.Down => new Vector2f(0f, 1f)
        };
        
        var currentTile = map[Position.Column, Position.Row];
        var nextTile = map[(uint)(Position.Column + positionChange.X), (uint)(Position.Row + positionChange.Y)];
        if (Position.Equals(currentTile.Position, EqualityType.XYBased) && nextTile.TileType is not TileType.Accessible)
        {
            MoveDirection = Direction.None;
            return;
        }

        Position = new Position(Position.X + positionChange.X, Position.Y + positionChange.Y);
        Sprite!.Position = new Vector2f(Position.X - Globals.TileSize / 2, Position.Y - Globals.TileSize / 2);
    }

    private void CheckTurnPossibility(Map map)
    {
        if (PlannedTurn is Direction.None)
            return;
        
        var positionChange = PlannedTurn switch
        {
            Direction.Left => new Vector2i(-1, 0),
            Direction.Up => new Vector2i(0, -1),
            Direction.Right => new Vector2i(1, 0),
            Direction.Down => new Vector2i(0, 1)
        };
        
        var currentTile = map[Position.Column, Position.Row];
        var nextTile = map[(uint)(Position.Column + positionChange.X), (uint)(Position.Row + positionChange.Y)];
        if (Position.Equals(currentTile.Position, EqualityType.XYBased) && nextTile.TileType is TileType.Accessible)
        {
            MoveDirection = PlannedTurn;
            PlannedTurn = Direction.None;
        }
    }
}


