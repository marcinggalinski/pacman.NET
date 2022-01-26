using pacman.NET.Config;
using pacman.NET.Types;
using SFML.Graphics;
using SFML.System;

namespace pacman.NET.Models;

public class Tile : Drawable
{
    private readonly Vertex[] _vertices;
    
    public Position Position { get; }

    public Tile(Position position)
    {
        Position = position;
        
        _vertices = new []
        {
            new Vertex(new Vector2f(Position.X - Constants.TileSize / 2, Position.Y - Constants.TileSize / 2)),
            new Vertex(new Vector2f(Position.X + Constants.TileSize / 2, Position.Y - Constants.TileSize / 2)),
            new Vertex(new Vector2f(Position.X + Constants.TileSize / 2, Position.Y + Constants.TileSize / 2)),
            new Vertex(new Vector2f(Position.X - Constants.TileSize / 2, Position.Y + Constants.TileSize / 2))
        };
        
        var v = new []
        {
            new Vertex(new Vector2f(20, 20)),
            new Vertex(new Vector2f(30, 20)),
            new Vertex(new Vector2f(30, 30)),
            new Vertex(new Vector2f(20, 30))
        };
    }

    public void Draw(RenderTarget target, RenderStates states)
    {
        target.Draw(_vertices, PrimitiveType.Quads);
    }
}
