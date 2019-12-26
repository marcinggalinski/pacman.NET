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
        public Position_t? FindTilePosition(Coords_t coords)
        {
            // coords of center of an actor
            var center = coords + new Coords_t(Defines.TileSize / 2);

            for(int c = 0; c < Width; c++)
            {
                for(int r = 0; r < Height; r++)
                {
                    //coords of top left corner of tile
                    var corner = new Coords_t(tiles[c, r].Coords.X, tiles[c, r].Coords.Y);

                    if(center.X > corner.X && center.X < corner.X + Defines.TileSize
                        && center.Y > corner.Y && center.Y < corner.Y + Defines.TileSize)
                    {
                        if(tiles[c, r].Content == TileContent.Wall)
                        {
                            return null;
                        }
                        else
                        {
                            return new Position_t(c, r);
                        }
                    }
                }
            }
            return null;
        }
    }
}
