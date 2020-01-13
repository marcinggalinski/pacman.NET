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
            var blinky = new Blinky(map, player);
            var pinky = new Pinky(map, player);
            var inky = new Inky(map, player, blinky);
            var clyde = new Clyde(map, player);

            Handlers.SetupGameEvents(window, player);

            var clock = new Clock();
            var previousTime = Time.FromMilliseconds(0);
            var currentTime = new Time();
            var dt = new Time();
            while(window.IsOpen)
            {
                currentTime = clock.ElapsedTime;
                dt = currentTime - previousTime;
                if(dt.AsMilliseconds() < 1000.0/Settings.MaxFPS)
                    continue;

                previousTime = currentTime;

                window.DispatchEvents();
                window.Clear();

                player.Move();
                blinky.Move();
                pinky.Move();
                inky.Move();
                clyde.Move();

                map.Draw(window);
                player.Draw(window);
                blinky.Draw(window);
                pinky.Draw(window);
                inky.Draw(window);
                clyde.Draw(window);
                
                window.Display();
            }
        }
    }
}
