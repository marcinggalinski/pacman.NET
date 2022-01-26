using pacman.NET.Config;
using pacman.NET.Types;
using SFML.Graphics;
using SFML.System;

namespace pacman.NET.Models;

public class Tile : Drawable
{
    private readonly Vertex[] _vertices;
    
    public Position Position { get; }
    public TileType TileType { get; }

    public Tile(Position position, TileType tileType)
    {
        Position = position;
        TileType = tileType;
        
        _vertices = new []
        {
            new Vertex(new Vector2f(Position.X - Constants.TileSize / 2, Position.Y - Constants.TileSize / 2)),
            new Vertex(new Vector2f(Position.X + Constants.TileSize / 2, Position.Y - Constants.TileSize / 2)),
            new Vertex(new Vector2f(Position.X + Constants.TileSize / 2, Position.Y + Constants.TileSize / 2)),
            new Vertex(new Vector2f(Position.X - Constants.TileSize / 2, Position.Y + Constants.TileSize / 2))
        };
    }

    public void Draw(RenderTarget target, RenderStates states)
    {
        if (TileType is TileType.Empty)
            return;
        target.Draw(_vertices, PrimitiveType.Quads);
    }
}

public enum TileType
{
    Empty,
    Wall,
    GhostHouseDoor
}
