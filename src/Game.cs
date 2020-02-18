using Pacman.Config;
using Pacman.Utilities;
using Pacman.Entities;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System;

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
            var player = new Player(map, 3);
            var blinky = new Blinky(map);
            var pinky = new Pinky(map);
            var inky = new Inky(map);
            var clyde = new Clyde(map);
            var hud = new Hud(player);

            map.SetPointers(player, blinky, pinky, inky, clyde);

            Handlers.SetupGameEvents(window, player);

            var level = 1;
            MapData.LoadLevel(level);

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

                hud.Draw(window);
                map.Draw(window);
                player.Draw(window);
                blinky.Draw(window);
                pinky.Draw(window);
                inky.Draw(window);
                clyde.Draw(window);
                
                window.Display();

                if(player.IsDead)
                {
                    if(player.Lives > 0)
                    {
                        Console.WriteLine(player.Lives);
                        player.Respawn();
                        blinky.Respawn();
                        pinky.Respawn();
                        inky.Respawn();
                        clyde.Respawn();
                    }
                    else
                    {
                        Console.WriteLine("GAME OVER");
                        window.Close();
                    }
                }
                
                if(map.Counter == MapData.NOfDots)
                {
                    System.Console.WriteLine("WEEEE!");
                    MapData.LoadLevel(++level);
                    map.Reset();
                }
            }
        }
    }
}
