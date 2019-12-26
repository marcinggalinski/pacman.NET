using Pacman.Config;
using Pacman.Utilities;
using SFML.Graphics;

namespace Pacman.Entities
{
    public abstract class Actor
    {
        // properties
        public Coords_t Coords { get; set; }
        public Position_t? Position { get; set; }
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
        protected bool IsAtBorder()
        {
            if(Position == null)
                return false;
            Tile tile = Map[Position.Value];
            return tile.Position.Column == 0 || tile.Position.Column == Map.Width - 1
                    || tile.Position.Row == 0 || tile.Position.Row == Map.Height - 1;
        }
    }
}
