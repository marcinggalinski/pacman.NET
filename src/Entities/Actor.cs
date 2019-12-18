using Pacman.Config;
using Pacman.Utilities;
using SFML.Graphics;

namespace Pacman.Entities
{
    public abstract class Actor
    {
        // properties
        public Coords_t Coords { get; set; }
        public Position_t Position { get; set; }
        public Direction MoveDirection { get; protected set; }
        public bool HasMoved { get; protected set; }
        protected Sprite Sprite { get; set; }
        protected Map Map { get; set; }
        // protected Player player { get; set; }
        // protected Ghost Blinky { get; set; }
        // protected Ghost Pinky { get; set; }
        // protected Ghost Inky { get; set; }
        // protected Ghost Clyde { get; set; }

        // public methods
        public abstract void Move();
        public void Draw(RenderWindow window)
        {
            window.Draw(Sprite);
        }

        // private methods
        protected bool FindTilePosition(Coords_t coords)
        {
            // coords of center of an actor
            var center = coords + new Coords_t(Defines.TileSize / 2);

            for(int c = 0; c < Map.Width; c++)
            {
                for(int r = 0; r < Map.Height; r++)
                {
                    //coords of top left corner of tile
                    var corner = new Coords_t(Map[c, r].Coords.X, Map[c, r].Coords.Y);

                    if(center.X > corner.X && center.X < corner.X + Defines.TileSize
                        && center.Y > corner.Y && center.Y < corner.Y + Defines.TileSize)
                    {
                        if(Map[c, r].Content == TileContent.Wall)
                        {
                            return false;
                        }
                        else
                        {
                            Position = new Position_t(c, r);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        protected bool IsAtBorder()
        {
            Tile tile = Map[Position];
            return tile.Position.Column == 0 || tile.Position.Column == Map.Width - 1
                    || tile.Position.Row == 0 || tile.Position.Row == Map.Height - 1;
        }
    }
}
