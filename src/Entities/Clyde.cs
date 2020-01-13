using System;
using Pacman.Config;
using Pacman.Utilities;

namespace Pacman.Entities
{
    public class Clyde : Ghost
    {
        public Clyde(Map map, Player player) : base(map, player, Textures.Clyde, MapData.Clyde)
        {
            Coords = MapData.Clyde.SpawnCoords;
            RespawnPosition = MapData.Clyde.RespawnPosition;
            Sprite.Position = Coords;
        }

        protected override Position_t GetDestination()
        {
            if(IsDead)
                return RespawnPosition;
            return Functions.Distance(Player.Position.Value, Position.Value) < 8 ?
                    new Position_t(0, Map.Height) : Player.Position.Value;
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
