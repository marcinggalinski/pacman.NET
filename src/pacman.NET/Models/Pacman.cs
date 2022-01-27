using pacman.NET.Config;
using pacman.NET.Types;
using SFML.Graphics;
using SFML.System;

namespace pacman.NET.Models;

public class Pacman : ModelBase
{
    public MoveDirection MoveDirection { get; private set; } = MoveDirection.None;

    public Pacman(Position startPosition)
    {
        Position = startPosition;
        Sprite = new Sprite(Globals.Textures.Pacman)
        {
            Position = new Vector2f(Position.X - Globals.TileSize / 2, Position.Y - Globals.TileSize / 2),
            Scale = new Vector2f(Globals.TileSize / Globals.Textures.Pacman.Size.X, Globals.TileSize / Globals.Textures.Pacman.Size.Y)
        };
    }
}


