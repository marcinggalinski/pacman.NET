using Pacman.Config;
using Pacman.Utilities;
using Pacman.Entities;
using SFML.Graphics;
using SFML.Window;

namespace Pacman
{
    public static class Game
    {
        public static void Main()
        {
            Settings.Load();
            Textures.Load();
            MapData.Load();

            var window = new RenderWindow(
                new VideoMode(Settings.Resolution.Width, Settings.Resolution.Height),
                "Pacman.NET");
            
            var map = new Map();
            while(window.IsOpen)
            {
                window.Clear();

                map.Draw(window);

                window.Display();
            }
        }
    }
}
