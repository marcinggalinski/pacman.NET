using Pacman.Config;
using Pacman.Utilities;
using Pacman.Entities;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System;
using System.Diagnostics;

namespace Pacman
{
    public static class Game
    {
        // properties and variables
        private static RenderWindow Window { get; set; }
        private static Hud Hud { get; set; }
        private static Map Map { get; set; }
        private static Player Player { get; set; }
        private static Blinky Blinky { get; set; }
        private static Pinky Pinky { get; set; }
        private static Inky Inky { get; set; }
        private static Clyde Clyde { get; set; }
        private static Clock Clock { get; set; } = new Clock();
        private static Stopwatch MessageTimer { get; set; } = new Stopwatch();

        private static uint level = 1;

        // methods
        public static void Initialize()
        {
            Settings.Load();
            Textures.Load();
            MapData.Load();

            Window = new RenderWindow(
                new VideoMode(Settings.Resolution.Width, Settings.Resolution.Height),
                "Pacman.NET");
            
            Map = new Map();
            Player = new Player(Map, 3);
            Blinky = new Blinky(Map);
            Pinky = new Pinky(Map);
            Inky = new Inky(Map);
            Clyde = new Clyde(Map);
            Hud = new Hud(Player);

            Map.SetPointers(Player, Blinky, Pinky, Inky, Clyde);
            Handlers.SetupGameEvents(Window, Player);
            MapData.LoadLevel(level);
        }

        private static void DisplayMessage(string message, uint millisecs)
        {
            Window.Clear();

            Hud.Draw(Window, level);
            Map.Draw(Window);
            Player.Draw(Window);
            Blinky.Draw(Window);
            Pinky.Draw(Window);
            Inky.Draw(Window);
            Clyde.Draw(Window);
            MessageScreen.Draw(Window, message);

            Window.Display();

            Handlers.BlockGameEvents = true;
            MessageTimer.Start();

            while(MessageTimer.ElapsedMilliseconds < millisecs)
                Window.DispatchEvents();

            MessageTimer.Reset();
            Handlers.BlockGameEvents = false;
        }

        private static void GameLoop()
        {
            var previousTime = Time.FromMilliseconds(0);
            var currentTime = new Time();
            var dt = new Time();
            
            while(Window.IsOpen)
            {
                currentTime = Clock.ElapsedTime;
                dt = currentTime - previousTime;
                if(dt.AsMilliseconds() < 1000.0/Settings.MaxFPS)
                    continue;

                previousTime = currentTime;

                Window.DispatchEvents();
                Window.Clear();

                Player.Move();
                Blinky.Move();
                Pinky.Move();
                Inky.Move();
                Clyde.Move();

                Hud.Draw(Window, level);
                Map.Draw(Window);
                Player.Draw(Window);
                Blinky.Draw(Window);
                Pinky.Draw(Window);
                Inky.Draw(Window);
                Clyde.Draw(Window);
                
                Window.Display();

                if(Player.IsDead)
                {
                    if(Player.Lives > 0)
                    {
                        Console.WriteLine(Player.Lives);
                        Player.Respawn();
                        Blinky.Respawn();
                        Pinky.Respawn();
                        Inky.Respawn();
                        Clyde.Respawn();

                        if(Player.Lives > 1)
                            DisplayMessage($"{Player.Lives} lives left", 1000);
                        else
                            DisplayMessage("1 live left", 1000);
                    }
                    else
                    {
                        Console.WriteLine("GAME OVER");
                        DisplayMessage("GAME OVER", 1000);
                        Window.Close();
                    }
                }
                
                if(Map.Counter == MapData.NOfDots)
                {
                    System.Console.WriteLine("WEEEE!");
                    MapData.LoadLevel(++level);
                    Map.Reset();
                    DisplayMessage($"Level {level}", 1000);
                }
            }
        }

        public static void Main()
        {
            Initialize();
            DisplayMessage($"Level {level}", 1000);
            GameLoop();
        }
    }
}
