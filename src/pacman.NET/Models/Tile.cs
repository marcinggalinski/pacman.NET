﻿using pacman.NET.Config;
using pacman.NET.Types;
using SFML.Graphics;
using SFML.System;

namespace pacman.NET.Models;

public class Tile : ModelBase
{
    public TileType TileType { get; }
    public TileContent TileContent { get; }
    
    public Rectangle Rectangle => new Rectangle(
        new Vector2f(Position.X - Globals.TileSize / 2, Position.Y - Globals.TileSize / 2),
        new Vector2f(Position.X + Globals.TileSize / 2, Position.Y - Globals.TileSize / 2),
        new Vector2f(Position.X + Globals.TileSize / 2, Position.Y + Globals.TileSize / 2),
        new Vector2f(Position.X - Globals.TileSize / 2, Position.Y + Globals.TileSize / 2));

    public Tile(Position position, TileType tileType, TileContent tileContent)
    {
        Position = position;
        TileType = tileType;
        TileContent = tileContent;

        var texture = TileType switch
        {
            TileType.Accessible => TileContent switch
            {
                TileContent.Dot => Globals.Textures.Tile.Dot,
                TileContent.SuperDot => Globals.Textures.Tile.SuperDot,
                _ => null
            },
            TileType.Wall or TileType.GhostHouseDoor => Globals.Textures.Tile.Wall,
            _ => null
        };

        if (texture is not null)
        {
            Sprite = new Sprite(texture)
            {
                Position = Rectangle.TopLeft,
                Scale = new Vector2f(Globals.TileSize / texture.Size.X, Globals.TileSize / texture.Size.Y)
            };
        }
    }
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
