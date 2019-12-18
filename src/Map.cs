using System;
using Pacman.Config;
using Pacman.Entities;
using Pacman.Utilities;
using SFML.Graphics;

namespace Pacman
{
    public class Map
    {
        // variables
        private Tile[,] tiles;

        // properties
        public int Width { get; set; }
        public int Height { get; set; }

        // ctor
        public Map()
        {
            Width = MapData.Width;
            Height = MapData.Height;
            tiles = MapData.Tiles;
        }

        // indexer
        public Tile this[int c, int r]
        {
            get
            {
                if(c < 0 || c >= Width || r < 0 || r >= Height)
                    throw new InvalidTilePositionException(new Position_t(c, r));
                return tiles[c, r];
            }
            private set
            {
                if(c < 0 || c >= Width || r < 0 || r >= Height)
                    throw new InvalidTilePositionException(new Position_t(c, r));
                tiles[c, r] = value;
            }
        }
        public Tile this[Position_t pos]
        {
            get => this[pos.Column, pos.Row];
            set => this[pos.Column, pos.Row] = value;
        }

        // methods
        public void Draw(RenderWindow window)
        {
            foreach(var tile in tiles)
            {
                tile.Draw(window);
            }
        }
    }
}
