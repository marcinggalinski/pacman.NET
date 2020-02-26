using System;
using Pacman.Config;
using Pacman.Utilities;
using SFML.Graphics;
using SFML.System;

namespace Pacman.Entities
{
    public class Player : Actor
    {
        // properties
        public Direction FaceDirection { get; set; }
        public int Score { get; private set; }
        public int Lives { get; private set; }
        private bool isDead;
        public bool IsDead
        {
            get => isDead;
            set
            {
                if(value)
                    Lives--;
                isDead = value;
                Sprite.Position = Coords;
            }
        }
        public bool IsOnDrugs { get; private set; }
        private Clock Timer { get; set; }
        private int ghostEatenMultiplier = 0;

        public Player(Map map, int lives) : base(map, MapData.Pacman.SpawnCoords)
        {
            Sprite = new Sprite(Textures.Pacman);
            Sprite.Scale = new Vector2f(
                (float)Defines.TileSize / Textures.Pacman.Size.X,
                (float)Defines.TileSize / Textures.Pacman.Size.Y);
            Sprite.Position = Coords;
            Score = 0;
            Lives = lives;
            Timer = new Clock();
            IsDead = false;
            IsOnDrugs = false;
        }

        public void Respawn()
        {
            Respawn(MapData.Pacman.SpawnCoords);
            IsDead = false;
            IsOnDrugs = false;
        }

        public void Turn(Direction dir)
        {
            if(Position != null)
            {
                PlannedTurn = dir;
                return;
            }
            Position = Map.FindTilePosition(Coords + Directions.Table[(int)dir]);
            if(Position != null && !Map[Position.Value].IsWall())
            {
                PlannedTurn = dir;
                CheckTurn();
                if(MoveDirection == Direction.None)
                    Position = null;
                Map.Timer.Restart();
            }
        }

        private void Move(double distance)
        {
            // checking whether it's possible to turn
            CheckTurn();

            // to shorten conditions
            Tile currentTile = Map[Position];
            var direction = MoveDirection;
            var dir = (int)direction;

            // move
            // if in a tunel, go through it
            if(IsAtBorder() && currentTile.Content == TileContent.Tunel)
            {
                // but still don't go through walls
                try
                {
                    if((Map[Position + Directions.Table[dir]].IsWall())
                        && Coords.X + Directions.Table[dir].X >= currentTile.Coords.X - 0.5
                        && Coords.X + Directions.Table[dir].X <= currentTile.Coords.X + 0.5
                        && Coords.Y + Directions.Table[dir].Y >= currentTile.Coords.Y - 0.5
                        && Coords.Y + Directions.Table[dir].Y <= currentTile.Coords.Y + 0.5)
                    {
                        Coords = currentTile.Coords;
                        MoveDirection = Direction.None;
                        direction = MoveDirection;
                        dir = (int)direction;
                    }
                }
                catch(InvalidTilePositionException)
                {
                    //
                }

                // change coords
                Coords += Directions.Table[dir];

                // if passed edge of tile (and therefore edge of map), move to the other side
                var center = new Coords_t(Coords + new Coords_t(Defines.TileSize / 2));
                if(center.X < Map[0, 0].Coords.X
                    || center.X > Map[Map.Width - 1, 0].Coords.X + Defines.TileSize
                    || center.Y < Map[0, 0].Coords.Y
                    || center.Y > Map[0, Map.Height - 1].Coords.Y + Defines.TileSize)
                {
                    Position = (Position + new Position_t(Map.Width, Map.Height) + Directions.Table[dir])
                                % new Vector2i(Map.Width, Map.Height);
                    Coords = Map[Position].Coords + currentTile.Coords - Coords;
                }
                else if(center.X < currentTile.Coords.X
                        || center.X > currentTile.Coords.X + Defines.TileSize
                        || center.Y < currentTile.Coords.Y
                        || center.Y > currentTile.Coords.Y + Defines.TileSize)
                {
                    Position += Directions.Table[dir];
                }
            }
            // if about to hit wall, well, don't
            else if((Map[Position + Directions.Table[dir]].Content == TileContent.Wall
                        || Map[Position + Directions.Table[dir]].Content == TileContent.GhosthouseDoor)
                    && Coords.X + Directions.Table[dir].X >= currentTile.Coords.X - 0.5
                    && Coords.X + Directions.Table[dir].X <= currentTile.Coords.X + 0.5
                    && Coords.Y + Directions.Table[dir].Y >= currentTile.Coords.Y - 0.5
                    && Coords.Y + Directions.Table[dir].Y <= currentTile.Coords.Y + 0.5)
            {
                Coords = currentTile.Coords;
                MoveDirection = Direction.None;
            }
            // otherwise just go
            else
            {
                Coords += Directions.Table[dir];
                var center = Coords + new Coords_t(Defines.TileSize / 2);
                if(center.X < currentTile.Coords.X
                    || center.X > currentTile.Coords.X + Defines.TileSize
                    || center.Y < currentTile.Coords.Y
                    || center.Y > currentTile.Coords.Y + Defines.TileSize)
                {
                    Position += Directions.Table[dir];
                }
            }

            var previousTile = currentTile;
            currentTile = Map[Position];
            if(previousTile == currentTile)
                return;

            if(currentTile.Content == TileContent.Dot)
            {
                Score += 10;
                Map.Timer.Restart();
                Map.Counter++;
            }
            else if(currentTile.Content == TileContent.SuperDot)
            {
                Score += 50;
                Map.Timer.Restart();
                Map.Counter++;
                IsOnDrugs = true;
                ghostEatenMultiplier = 0;

                Map.Blinky.Mode = GhostMode.Frightened;
                Map.Pinky.Mode = GhostMode.Frightened;
                Map.Inky.Mode = GhostMode.Frightened;
                Map.Clyde.Mode = GhostMode.Frightened;

                Timer.Restart();
            }

            previousTile.SetPacman(false);
            currentTile.SetPacman(true);

            CheckGhosts();
            if(IsDead)
                return;
        }

        public override void Move(Time dt)
        {
            if(Position == null)
                return;

            if(IsOnDrugs && Timer.ElapsedTime.AsMilliseconds() >= 7000)
            {
                IsOnDrugs = false;
                if(Map.Blinky.Mode == GhostMode.Frightened)
                    Map.Blinky.Mode = GhostMode.Chase;
                if(Map.Pinky.Mode == GhostMode.Frightened)
                    Map.Pinky.Mode = GhostMode.Chase;
                if(Map.Inky.Mode == GhostMode.Frightened)
                    Map.Inky.Mode = GhostMode.Chase;
                if(Map.Clyde.Mode == GhostMode.Frightened)
                    Map.Clyde.Mode = GhostMode.Chase;
            }

            var distance = Defines.BaseSpeed * Defines.TileSize * dt.AsMilliseconds() / 1000.0
                       * (IsOnDrugs ? Defines.PacmanSpeed.OnDrugs : Defines.PacmanSpeed.Normal);

            for(; distance > 1.0; distance--)
            {
                Move(1.0);
                if(IsDead)
                    return;
            }
            Move(distance);
            Sprite.Position = Coords;
        }
        public void CheckGhosts()
        {
            var currentTile = Map[Position];
            if(currentTile.GhostsContaining > 0)
            {
                var ghosts = Map.GhostsInTile(currentTile.Position);
                foreach(var ghost in ghosts)
                {
                    if(ghost.Mode == GhostMode.Dead)
                        continue;
                    if(ghost.Mode == GhostMode.Frightened)
                    {
                        ghostEatenMultiplier = (ghostEatenMultiplier == 0 ? 1 : 2 * ghostEatenMultiplier);
                        Score += ghostEatenMultiplier * 200;
                        ghost.Mode = GhostMode.Dead;
                        continue;
                    }
                    IsDead = true;
                    break;
                }
            }
        }
        private void CheckTurn()
        {
            // to shorten conditions
            Tile currentTile = Map[Position];
            Direction direction = PlannedTurn;
            int dir = (int)direction;
            
            // checking whether it's possible to turn
            try
            {
                // if it's possible, turn
                if(!Map[Position + Directions.Table[dir]].IsWall())
                {
                    if(MoveDirection == Direction.None
                       || Directions.Table[dir] == -Directions.Table[(int)MoveDirection])
                    {
                        MoveDirection = direction;
                        FaceDirection = direction;
                    }
                    else if(Coords.X >= currentTile.Coords.X - 0.5
                            && Coords.X <= currentTile.Coords.X + 0.5
                            && Coords.Y >= currentTile.Coords.Y - 0.5
                            && Coords.Y <= currentTile.Coords.Y + 0.5
                            && direction != MoveDirection)
                    {
                        Coords = currentTile.Coords;
                        MoveDirection = direction;
                        FaceDirection = direction;
                    }
                }
            }
            catch(InvalidTilePositionException)
            {
                if(currentTile.Content == TileContent.Tunel
                   && Coords.X >= currentTile.Coords.X - 0.5
                   && Coords.X <= currentTile.Coords.X + 0.5
                   && Coords.Y >= currentTile.Coords.Y - 0.5
                   && Coords.Y <= currentTile.Coords.Y + 0.5
                   && direction != MoveDirection)
                {
                    Coords = currentTile.Coords;
                    MoveDirection = direction;
                    FaceDirection = direction;
                }
            }
        }
    }
}
