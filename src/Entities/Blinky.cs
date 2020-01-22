using Pacman.Config;
using Pacman.Utilities;

namespace Pacman.Entities
{
    public class Blinky : Ghost
    {
        public Blinky(Map map) : base(map, Textures.Blinky, MapData.Blinky)
        {
            Coords = MapData.Blinky.SpawnCoords;
            RespawnPosition = MapData.Blinky.RespawnPosition;
            Sprite.Position = Coords;
            IsOut = true;
            Mode = GhostMode.Chase;
        }
        protected override Position_t GetDestination() => Mode == GhostMode.Dead ? RespawnPosition : Map.Player.Position.Value;
        protected override bool MayLeave() => WasDead ? Timer.ElapsedTime.AsMilliseconds() > 1000 : true;
    }
}
