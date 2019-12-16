using Pacman.Config;
using Pacman.Utilities;
using SFML.Graphics;
using SFML.System;

namespace Pacman.Entities
{
    public class Tile
    {
        // properties
        public Position_t Position { get; set; }
        public Coords_t Coords { get; set; }
        public TileContent Content { get; set; }
        public bool ContainsPacman { get; private set; }
        public bool ContainsGhost { get; private set; }
        private Sprite Sprite { get; set; }

        // ctors
        public Tile(Position_t position, TileContent content)
        {
            Position = position;
            Coords = new Coords_t(Defines.SideMargin + position.Column * Defines.TileSize,
                            Defines.TopMargin + Defines.HudMargin + position.Row * Defines.TileSize);
            Content = content;
            Sprite = null;
        }
        public Tile(Position_t position, TileContent content, Texture texture) : this(position, content)
        {
            Sprite = new Sprite(texture);
            Sprite.Scale = new Vector2f(
                            (float)Defines.TileSize / texture.Size.X,
                            (float)Defines.TileSize / texture.Size.Y);
            Sprite.Position = new Vector2f(
                                (float)Coords.X,
                                (float)Coords.Y);
        }

        // methods
        public void Draw(RenderWindow window)
        {
            if(Sprite != null)
                window.Draw(Sprite);
        }
    }
}
