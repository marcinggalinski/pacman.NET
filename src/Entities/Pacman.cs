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
        public Direction PlannedTurn { get; set; }
        public Direction FaceDirection { get; set; }
        public int Score { get; private set; }

        public Player(Map map)
        {
            Coords = MapData.Pacman;
            Sprite = new Sprite(Textures.Pacman);
            Sprite.Scale = new Vector2f(
                (float)Defines.TileSize / Textures.Pacman.Size.X,
                (float)Defines.TileSize / Textures.Pacman.Size.Y);
            Sprite.Position = Coords;
            Map = map;
            Score = 0;
        }
        public void Turn(Direction dir)
        {
            if(HasMoved)
            {
                PlannedTurn = dir;
                return;
            }
            var position = Map.FindTilePosition(Coords + Directions.Table[(int)dir]);
            if(position != null)
            {
                Position = (Position_t)position;
                PlannedTurn = dir;
                MoveDirection = dir;
                FaceDirection = dir;
                HasMoved = true;
            }
        }
        public override void Move()
        {
            if(Position == null)
                return;

            for(var move = Defines.BaseSpeed; move > 0; move--)
            {                
                // checking whether it's possible to turn
                checkTurn();

                // to shorten conditions
                Tile currentTile = Map[Position.Value];
                var direction = MoveDirection;
                var dir = (int)direction;

                // move
                // if in a tunel, go through it
                if(IsAtBorder() && currentTile.Content == TileContent.Tunel)
                {
                    // but still don't go through walls
                    try
                    {
                        if((Map[Position + Directions.Table[dir]].Content == TileContent.Wall
                                || Map[Position + Directions.Table[dir]].Content == TileContent.GhosthouseDoor)
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
                        Coords = Map[Position.Value].Coords + currentTile.Coords - Coords;
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
                currentTile = Map[Position.Value];
                if(previousTile.Equals(currentTile) == false)
                    continue;

                if(currentTile.Content == TileContent.Dot)
                {
                    Score += 10;
                }
                else if(currentTile.Content == TileContent.SuperDot)
                {
                    Score += 50;
                    // on drugs!!!
                }

                previousTile.SetPacman(false);
                currentTile.SetPacman(true);

                if(currentTile.ContainsGhost)
                {
                    throw new NotImplementedException();
                }
            }
            Sprite.Position = Coords;
        }
        private void checkTurn()
        {
            // to shorten conditions
            Tile currentTile = Map[Position.Value];
            Direction direction = PlannedTurn;
            int dir = (int)direction;
            
            // checking whether it's possible to turn
            try
            {
                // if it's possible to turn
                if(Map[Position.Value + Directions.Table[dir]].Content != TileContent.Wall
                    && Map[Position + Directions.Table[dir]].Content != TileContent.GhosthouseDoor)
                {
                    if(Directions.Table[dir] == -Directions.Table[(int)MoveDirection])
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
