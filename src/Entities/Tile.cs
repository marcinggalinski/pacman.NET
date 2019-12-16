using pacman.Config;
using pacman.Utilities;
using SFML.Graphics;

namespace pacman.Entities
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

        // ctor
        public Tile(Position_t position, TileContent content, Texture texture = null)
        {
            Position = position;
            Coords = new Coords_t(Defines.SideMargin + position.Column * Defines.TileSize,
                            Defines.TopMargin + Defines.HudMargin + position.Row * Defines.TileSize);
            Content = content;
            Sprite = (texture == null ? null : new Sprite(texture));
        }

        // methods
        public void Draw(RenderWindow window)
        {
            if(Sprite != null)
                window.Draw(Sprite);
        }
    }
}
