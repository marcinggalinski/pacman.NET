using Pacman.Config;
using Pacman.Utilities;
using Pacman.Entities;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

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
            var player = new Player(map);

            Handlers.SetupGameEvents(window, player);

            var clock = new Clock();
            var previousTime = Time.FromMilliseconds(0);
            var currentTime = new Time();
            var dt = new Time();
            while(window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear();

                player.Move();

                map.Draw(window);
                player.Draw(window);
                
                currentTime = clock.ElapsedTime;
                dt = currentTime - previousTime;

                if(dt.AsMilliseconds() < 1000.0/60.0)
                    continue;

                previousTime = currentTime;

                window.Display();
            }
        }
    }
}
