using pacman.Config;
using pacman.Utilities;
using pacman.Entities;

namespace pacman
{
    public static class Game
    {
        public static void Main()
        {
            Settings.Load();
            Textures.Load();
            MapData.Load();
        }
    }
}
