using System;
using Pacman.Config;
using Pacman.Entities;
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
                if(c < 0 || c > Width || r < 0 || r > Height)
                    throw new ArgumentException();
                return tiles[c, r];
            }
            private set
            {
                if(c < 0 || c > Width || r < 0 || r > Height)
                    throw new ArgumentException();
                tiles[c, r] = value;
            }
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
