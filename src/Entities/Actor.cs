using System;
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
        public Direction PlannedTurn { get; protected set; }
        protected Sprite Sprite { get; set; }
        protected Map Map { get; set; }

        public Actor(Map map, Coords_t coords)
        {
            Map = map;
            Coords = coords;
        }

        public void Respawn(Coords_t coords)
        {
            Coords = coords;
            Position = null;
            Sprite.Position = Coords;
            PlannedTurn = Direction.None;
            MoveDirection = Direction.None;
        }

        // public methods
        public abstract void Move();
        public void Draw(RenderWindow window)
        {
            window.Draw(Sprite);
            // Console.WriteLine("{0} position {1}", this.GetType(), Position);
        }

        // private methods
        protected bool IsAtBorder()
        {
            if(Position == null)
                return false;
            Tile tile = Map[Position];
            return tile.Position.Column == 0 || tile.Position.Column == Map.Width - 1
                    || tile.Position.Row == 0 || tile.Position.Row == Map.Height - 1;
        }
    }
}
