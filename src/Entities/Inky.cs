using System;
using Pacman.Config;
using Pacman.Utilities;

namespace Pacman.Entities
{
    public class Inky : Ghost
    {
        public Inky(Map map) : base(map, Textures.Inky, MapData.Inky)
        {
            Coords = MapData.Inky.SpawnCoords;
            RespawnPosition = MapData.Inky.RespawnPosition;
            Sprite.Position = Coords;
        }
        protected override Position_t GetDestination()
        {
            if(Mode == GhostMode.Dead)
                return RespawnPosition;
            else
            {
                var offsetPosition = Map.Player.Position + 2 * Directions.Table[(int)Map.Player.FaceDirection];
                var diff = offsetPosition - Map.Blinky.Position.Value;

                return offsetPosition + diff;
            }
        }
        protected override bool MayLeave()
        {
            if(WasDead)
                return Timer.ElapsedTime.AsMilliseconds() > 1000;

            if(Timer.ElapsedTime.AsMilliseconds() > 4000
               && (Map.Timer.ElapsedTime.AsMilliseconds() > 4000
                   || Map.Counter >= 30))
            {
                Map.Timer.Restart();
                return true;
            }
            return false;
        }
    }
}
