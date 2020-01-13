using System;
using Pacman.Config;
using Pacman.Utilities;

namespace Pacman.Entities
{
    public class Inky : Ghost
    {
        private Blinky Blinky { get; set; }
        public Inky(Map map, Player player, Blinky blinky) : base(map, player, Textures.Inky, MapData.Inky)
        {
            Coords = MapData.Inky.SpawnCoords;
            RespawnPosition = MapData.Inky.RespawnPosition;
            Sprite.Position = Coords;
            Blinky = blinky;
        }
        protected override Position_t GetDestination()
        {
            if(IsDead)
                return RespawnPosition;
            else
            {
                var offsetPosition = Player.Position + 2 * Directions.Table[(int)Player.FaceDirection];
                var diff = offsetPosition - Blinky.Position.Value;

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
