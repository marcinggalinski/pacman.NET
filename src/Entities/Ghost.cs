using System;
using System.Collections.Generic;
using Pacman.Config;
using Pacman.Utilities;
using SFML.Graphics;
using SFML.System;
using static Pacman.Config.MapData;
using static Pacman.Config.Textures;

namespace Pacman.Entities
{
    public abstract class Ghost : Actor
    {
        protected class GhostSprites
        {
            public Sprite Normal { get; set; } 
            public Sprite Frightened { get; set; }
            public Sprite Dead { get; set; }
        }

        protected Position_t RespawnPosition { get; set; }
        private GhostMode mode;
        public GhostMode Mode
        {
            get => mode;
            set
            {
                switch(value)
                {
                case GhostMode.Idle:
                    mode = value;
                    Sprite = Sprites.Normal;
                    return;
                case GhostMode.Scatter:
                    if(mode == GhostMode.Idle
                       || mode == GhostMode.Dead
                       || Map[Position].IsWall())
                        return;
                    mode = value;
                    Sprite = Sprites.Normal;
                    return;
                case GhostMode.Chase:
                    mode = value;
                    Sprite = Sprites.Normal;
                    return;
                case GhostMode.Frightened:
                    if(mode == GhostMode.Idle
                       || mode == GhostMode.Frightened
                       || mode == GhostMode.Dead
                       || Map[Position].Content == TileContent.Ghosthouse
                       || Map[Position].Content == TileContent.GhosthouseDoor)
                        return;
                    mode = value;
                    Sprite = Sprites.Frightened;
                    ChangedTile = true;
                    for(int i = 0; i < (int)Direction.NOfDirections; i++)
                    {
                        if(Directions.Table[i] == -Directions.Table[(int)MoveDirection])
                        {
                            MoveDirection = (Direction)i;
                            break;
                        }
                    }
                    return;
                case GhostMode.Dead:
                    mode = value;
                    Sprite = Sprites.Dead;
                    return;
                }
            }
        }
        protected GhostSprites Sprites { get; set; }
        protected int[,] ReachableTiles { get; set; }
        protected int[,] Distances { get; set; }
        private bool ChangedTile { get; set; }
        protected bool WasDead { get; set; }
        protected bool IsOut { get; set; }
        protected static Clock Timer { get; private set; }

        public Ghost(Map map, GhostTextures textures, ActorData data) : base(map, data.SpawnCoords)
        {
            ReachableTiles = MapData.IntMap;
            Distances = new int[map.Width, map.Height];

            Sprites = new GhostSprites();
            Sprites.Normal = new Sprite(textures.Normal);
            Sprites.Normal.Scale = new Vector2f(
                Defines.TileSize / textures.Normal.Size.X,
                Defines.TileSize / textures.Normal.Size.Y);
            Sprites.Frightened = new Sprite(textures.Frightened);
            Sprites.Frightened.Scale = new Vector2f(
                Defines.TileSize / textures.Frightened.Size.X,
                Defines.TileSize / textures.Frightened.Size.Y);
            Sprites.Dead = new Sprite(textures.Dead);
            Sprites.Dead.Scale = new Vector2f(
                Defines.TileSize / textures.Dead.Size.X,
                Defines.TileSize / textures.Dead.Size.Y);
            Sprite = Sprites.Normal;

            WasDead = false;
            IsOut = false;
            Mode = GhostMode.Idle;
            Timer = new Clock();
        }

        public void Respawn(MapData.ActorData data)
        {
            Mode = GhostMode.Idle;
            Sprite = Sprites.Normal;
            WasDead = false;
            IsOut = false;
            ChangedTile = false;
            Respawn(data.SpawnCoords);
        }

        private void CheckTurn()
        {
            // to shorten conditions
            Tile currentTile = Map[Position];
            Direction direction = PlannedTurn;
            int dir = (int)direction;
            var dirV = Directions.Table[dir];
            var mDirV = Directions.Table[(int)MoveDirection];

            //checking whether it's possible to turn
            try
            {
                // if it is, turn
                if(!Map[Position + dirV].IsWall()
                   && dirV == -mDirV)
                {
                    MoveDirection = direction;
                    ChangedTile = false;
                    return;
                }
                if((!Map[Position + dirV].IsWall()
                   || (Map[Position + dirV].Content == TileContent.GhosthouseDoor
                       && (Map[Position].Content == TileContent.Ghosthouse
                           || Mode == GhostMode.Dead)))
                   && ((dirV.X == 0
                        && Coords.X < currentTile.Coords.X + 0.5
                        && Coords.X > currentTile.Coords.X - 0.5)
                       || (dirV.Y == 0
                           && Coords.Y < currentTile.Coords.Y + 0.5
                           && Coords.Y > currentTile.Coords.Y - 0.5))
                   && direction != MoveDirection)
                {
                    Coords = currentTile.Coords;
                    MoveDirection = direction;
                    ChangedTile = false;
                    return;
                }
                if(Map[Position + mDirV].IsWall()
                   && Map[Position + mDirV].Content != TileContent.GhosthouseDoor
                   && (Map[Position + dirV].IsWall()
                       || Map[Position + dirV].Content == TileContent.GhosthouseDoor))
                {
                    MoveDirection = direction;
                    ChangedTile = false;
                    return;
                }
            }
            catch(InvalidTilePositionException)
            {
                if(currentTile.Content == TileContent.Tunel
                   && Coords.X < currentTile.Coords.X + 0.5
                   && Coords.X > currentTile.Coords.X - 0.5
                   && Coords.Y < currentTile.Coords.Y + 0.5
                   && Coords.Y > currentTile.Coords.Y - 0.5)
                {
                    Coords = currentTile.Coords;
                    MoveDirection = direction;
                }
            }
        }

        private void Move(double distance)
        {
            // possibly turn
            PlannedTurn = ChooseDirection();

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
                // but still don't go through the walls
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

                // update coords
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
                    ChangedTile = true;
                }
                else if(center.X < currentTile.Coords.X
                        || center.X > currentTile.Coords.X + Defines.TileSize
                        || center.Y < currentTile.Coords.Y
                        || center.Y > currentTile.Coords.Y + Defines.TileSize)
                {
                    Position += Directions.Table[dir];
                    ChangedTile = true;
                }
            }
            // if about to exit ghosthouse, dewit
            else if(Map[Position + Directions.Table[dir]].Content == TileContent.GhosthouseDoor
                    && IsOut)
            {
                Coords += Directions.Table[dir];
                var center = Coords + new Coords_t(Defines.TileSize / 2);
                if(center.X < currentTile.Coords.X
                    || center.X > currentTile.Coords.X + Defines.TileSize
                    || center.Y < currentTile.Coords.Y
                    || center.Y > currentTile.Coords.Y + Defines.TileSize)
                {
                    Position += Directions.Table[dir];
                    ChangedTile = true;
                }
            }
            // if about to hit the wall, don't
            else if(Map[Position + Directions.Table[dir]].IsWall()
                    && Coords.X + Directions.Table[dir].X < currentTile.Coords.X + 0.5
                    && Coords.X + Directions.Table[dir].X > currentTile.Coords.X - 0.5
                    && Coords.Y + Directions.Table[dir].Y < currentTile.Coords.Y + 0.5
                    && Coords.Y + Directions.Table[dir].Y > currentTile.Coords.Y - 0.5)
            {
                Coords = currentTile.Coords;
            }
            // otherwise the road is wide and clear for you to go
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
                    ChangedTile = true;
                }
            }

            var previousTile = currentTile;
            currentTile = Map[Position];
            if(previousTile == currentTile)
                return;
            
            if(Mode == GhostMode.Dead && Position == RespawnPosition)
            {
                Mode = GhostMode.Chase;
                Sprite = Sprites.Normal;
            }

            previousTile.GhostsContaining--;
            currentTile.GhostsContaining++;

            Map.Player.CheckGhosts();
        }

        public override void Move(Time dt)
        {
            if(Map.Player.Position == null)
                return;
            
            if(Mode == GhostMode.Idle)
            {
                if(MayLeave())
                {
                    IsOut = true;
                    Mode = GhostMode.Chase;
                }
                return;
            }

            if(Position == null)
            {
                for(int i = 0; i < (int)Direction.NOfDirections; i++)
                {
                    if((Direction)i == Direction.None)
                        continue;
                    Position = Map.FindTilePosition(Coords + Directions.Table[i]);
                    if(Position != null && !Map[Position.Value].IsWall())
                    {
                        ChangedTile = true;
                        break;
                    }
                }
            }

            var distance = Defines.BaseSpeed * Defines.TileSize * dt.AsMilliseconds() / 1000.0;
            if(Mode == GhostMode.Frightened)
                distance *= Defines.GhostSpeed.Frightened;
            else if(Map[Position].Content == TileContent.Tunel)
                distance *= Defines.GhostSpeed.InTunel;
            else
                distance *= Defines.GhostSpeed.Normal;

            for(; distance > 1.0; distance--)
            {
                if(Map.Player.IsDead)
                {
                    Sprite.Position = Coords;
                    return;
                }
                Move(1.0);
            }
            Move(distance);
            Sprite.Position = Coords;
        }

        protected abstract bool MayLeave();
        protected abstract Position_t GetDestination();

        private void ResetDistances()
        {
            for(int c = 0; c < Map.Width; c++)
                for(int r = 0; r < Map.Height; r++)
                    Distances[c, r] = Int32.MaxValue;
        }

        private void WallDijkstra(Queue<(Position_t pos, int dist)> tileQueue, Position_t destination)
        {
            var wallQueue = new Queue<(Position_t pos, int dist)>();
            wallQueue.Enqueue((destination, 0));

            int closestTileDist = Int32.MaxValue;
            bool foundTile = false;

            while(wallQueue.Count > 0)
            {
                var qEl = wallQueue.Dequeue();

                // if found better way to the tile, proceed
                if(qEl.dist < Distances[qEl.pos.Column, qEl.pos.Row])
                {
                    Distances[qEl.pos.Column, qEl.pos.Row] = qEl.dist;

                    foreach(var dir in Directions.Table)
                    {
                        // don't process tile that's off the map
                        if(Map.IsOffTheMap(qEl.pos + dir))
                            continue;
                        
                        // add closest to destination non-wall tile
                        if(ReachableTiles[qEl.pos.Column + (int)dir.X, qEl.pos.Row + (int)dir.Y] == 1
                           && qEl.dist + 1 <= closestTileDist)
                        {
                            foundTile = true;
                            closestTileDist = qEl.dist + 1;
                            tileQueue.Enqueue((qEl.pos + dir, closestTileDist));
                            continue;
                        }

                        // add wall adjacent in dir
                        if(!foundTile && ReachableTiles[qEl.pos.Column + (int)dir.X, qEl.pos.Row + (int)dir.Y] == 0)
                            wallQueue.Enqueue((qEl.pos + dir, qEl.dist + 1));
                    }
                }
            }
        }

        private void OffMapDijkstra(Queue<(Position_t, int)> tileQueue, Position_t destination)
        {
            var pos = destination;

            if(pos.Column < 0)
                pos.Column = 0;
            if(pos.Row < 0)
                pos.Row = 0;
            if(pos.Column >= Map.Width)
                pos.Column = Map.Width - 1;
            if(pos.Row >= Map.Height)
                pos.Row = Map.Height - 1;
            
            WallDijkstra(tileQueue, pos);
        }

        private void Dijkstra(Position_t destination)
        {
            ResetDistances();

            var tileQueue = new Queue<(Position_t pos, int dist)>();

            switch(destination)
            {
            case var d when Map.IsOffTheMap(destination):
                OffMapDijkstra(tileQueue, destination);
                break;
            case var d when ReachableTiles[destination.Column, destination.Row] == 0:
                WallDijkstra(tileQueue, destination);
                break;
            default:
                tileQueue.Enqueue((destination, 0));
                break;
            }

            while(tileQueue.Count > 0)
            {
                var qEl = tileQueue.Dequeue();

                // if found better path to tile, proceed
                if(qEl.dist < Distances[qEl.pos.Column, qEl.pos.Row])
                {
                    Distances[qEl.pos.Column, qEl.pos.Row] = qEl.dist;

                    foreach(var dir in Directions.Table)
                    {
                        // add tile on the other side of a tunel
                        if(Map[qEl.pos].Content == TileContent.Tunel
                           && (qEl.pos.Column + dir.X < 0
                               || qEl.pos.Column + dir.X >= Map.Width
                               || qEl.pos.Row + dir.Y < 0
                               || qEl.pos.Row + dir.Y >= Map.Height))
                        {
                            Position_t p = (qEl.pos + new Position_t(Map.Width, Map.Height) + dir)
                                                % new Position_t(Map.Width, Map.Height);
                            tileQueue.Enqueue((p, qEl.dist + 1));
                            continue;
                        }
                        // but do not add tile if it's off the map
                        if(Map.IsOffTheMap(qEl.pos + dir))
                            continue;
                        // add tile adjacent in dir if it's reachable
                        if(ReachableTiles[qEl.pos.Column + (int)dir.X, qEl.pos.Row + (int)dir.Y] == 1)
                            tileQueue.Enqueue((qEl.pos + dir, qEl.dist + 1));
                    }
                }
            }
        }
        private Direction ChooseDirection()
        {
            // don't turn if hadn't moved to another tile yet after last turn
            if(ChangedTile == false && MoveDirection != Direction.None)
                return PlannedTurn;
            
            Tile currentTile = Map[Position];

            bool oneWayIn = true;
            for(int dir = 0; dir < (int)Direction.NOfDirections; dir++)
            {
                if((Direction)dir == Direction.None)
                    continue;
                try
                {
                    if(!Map[Position + Directions.Table[dir]].IsWall()
                       && Directions.Table[dir] != -Directions.Table[(int)MoveDirection])
                       oneWayIn = false;
                }
                catch(InvalidTilePositionException)
                {
                    //
                }
            }

            if(oneWayIn)
                for(int dir = 0; dir < (int)Direction.NOfDirections; dir++)
                    if(Directions.Table[dir] == -Directions.Table[(int)MoveDirection])
                        return (Direction)dir;

            if(Mode == GhostMode.Frightened)
            {                
                Random rand = new Random();

                while(true)
                {
                    int dir = rand.Next(1, (int)Direction.NOfDirections);
                    try
                    {
                        if(Directions.Table[dir] == -Directions.Table[(int)MoveDirection]
                           || Map[Position + Directions.Table[dir]].IsWall())
                            continue;
                    }
                    catch(InvalidTilePositionException)
                    {
                        if(Directions.Table[dir] != -Directions.Table[(int)MoveDirection]
                           && currentTile.Content == TileContent.Tunel)
                            return (Direction)dir;
                    }
                    return (Direction)dir;
                }
            }
            // else
            var shortestDir = MoveDirection;
            int shortestLen = Int32.MaxValue;
            var backPos = (Position + new Position_t(Map.Width, Map.Height) - Directions.Table[(int)shortestDir])
                          % new Position_t(Map.Width, Map.Height);
            
            if(ReachableTiles[backPos.Column, backPos.Row] == 0)
                Dijkstra(GetDestination());
            else
            {
                ReachableTiles[backPos.Column, backPos.Row] = 2;
                Dijkstra(GetDestination());
                ReachableTiles[backPos.Column, backPos.Row] = 1;
            }

            for(int i = 0; i < (int)Direction.NOfDirections; i++)
            {
                try
                {
                    if((Direction)i == Direction.None)
                        continue;

                    var dir = Directions.Table[i];
                    if(dir == -Directions.Table[(int)MoveDirection])
                        continue;
                    
                    var pos = (Position + new Position_t(Map.Width, Map.Height) + dir)
                               % new Position_t(Map.Width, Map.Height);
                    
                    if((!Map[pos].IsWall()
                        || (Map[pos].Content == TileContent.GhosthouseDoor
                            && (Map[Position].Content == TileContent.Ghosthouse
                                || Mode == GhostMode.Dead)))
                       && Distances[pos.Column, pos.Row] < shortestLen)
                    {
                        shortestLen = Distances[pos.Column, pos.Row];
                        shortestDir = (Direction)i;
                        ChangedTile = false;
                    }
                }
                catch(InvalidTilePositionException)
                {
                    //
                }
            }
            return shortestDir;
        }
    }
}
