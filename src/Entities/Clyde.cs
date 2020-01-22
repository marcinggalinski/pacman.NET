using System;
using Pacman.Config;
using Pacman.Utilities;

namespace Pacman.Entities
{
    public class Clyde : Ghost
    {
        public Clyde(Map map) : base(map, Textures.Clyde, MapData.Clyde)
        {
            Coords = MapData.Clyde.SpawnCoords;
            RespawnPosition = MapData.Clyde.RespawnPosition;
            Sprite.Position = Coords;
        }

        protected override Position_t GetDestination()
        {
            if(Mode == GhostMode.Dead)
                return RespawnPosition;
            return Functions.Distance(Map.Player.Position.Value, Position.Value) < 8 ?
                    new Position_t(0, Map.Height) : Map.Player.Position.Value;
        }
        protected override bool MayLeave()
        {
            if(WasDead)
                return Timer.ElapsedTime.AsMilliseconds() > 1000;
            if(Timer.ElapsedTime.AsMilliseconds() > 8000
               && (Map.Timer.ElapsedTime.AsMilliseconds() > 4000
                   || Map.Counter >= 60))
            {
                Map.Timer.Restart();
                return true;
            }
            return false;
        }
    }
}
