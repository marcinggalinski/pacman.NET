using Pacman.Config;
using Pacman.Utilities;

namespace Pacman.Entities
{
    public class Pinky : Ghost
    {
        public Pinky(Map map) : base(map, Textures.Pinky, MapData.Pinky)
        {
            Coords = MapData.Pinky.SpawnCoords;
            RespawnPosition = MapData.Pinky.RespawnPosition;
            Sprite.Position = Coords;
        }
        protected override Position_t GetDestination()
        {
            return Mode == GhostMode.Dead ? RespawnPosition : Map.Player.Position + 4 * Directions.Table[(int)Map.Player.FaceDirection];
        }
        protected override bool MayLeave()
        {
            if(WasDead)
                return Timer.ElapsedTime.AsMilliseconds() > 1000;
            if(Timer.ElapsedTime.AsMilliseconds() > 1000)
            {
                Map.Timer.Restart();
                return true;
            }
            return false;
        }
    }
}
