﻿using SFML.Graphics;

namespace pacman.NET.Config;

public static class Globals
{
    public static uint WindowWidth { get; set; } = 800;
    public static uint WindowHeight { get; set; } = 600;
    
    public static uint TileSize { get; set; }
    public static uint HalfTileSize => TileSize / 2;
    public static uint TopMargin { get; set; }
    public static uint LeftMargin { get; set; }
    
    public static float MoveUnit { get; set; }

    public static class Textures
    {
        public static class Tile
        {
            public static Texture Wall { get; set; } = null!;
            public static Texture Dot { get; set; } = null!;
            public static Texture SuperDot { get; set; } = null!;
        }

        public static Texture Pacman = null!;
    }
}
