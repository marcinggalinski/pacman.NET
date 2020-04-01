using Pacman.Config;
using Pacman.Utilities;
using Pacman.Entities;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System;
using System.Diagnostics;
using System.Threading;

namespace Pacman
{
    public static class Game
    {
        // properties and variables
        private static RenderWindow Window { get; set; }
        private static Mutex WindowMutex { get; set; } = new Mutex();
        private static Hud Hud { get; set; }
        private static Map Map { get; set; }
        private static Player Player { get; set; }
        private static Blinky Blinky { get; set; }
        private static Pinky Pinky { get; set; }
        private static Inky Inky { get; set; }
        private static Clyde Clyde { get; set; }
        private static Clock Clock { get; set; } = new Clock();
        private static Stopwatch MessageTimer { get; set; } = new Stopwatch();
        public static bool IsPaused { get; set; } = false;
        public static bool QuitGameScreen { get; set; } = false;
        public static bool Ended { get; set; } = false;

        private static uint level;


        // methods
        public static void Initialize(RenderWindow window)
        {
            level = 1;
            Ended = false;
            IsPaused = false;

            Textures.Load();
            MapData.Load();

            Window = window;

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
            Clock.Restart();
            
            while(!Ended && Window.IsOpen)
            {
                WindowMutex.WaitOne();

                currentTime = Clock.ElapsedTime;
                dt = currentTime - previousTime;

                Window.Clear();

                if(!IsPaused)
                {
                    Player.Move(dt);
                    Blinky.Move(dt);
                    Pinky.Move(dt);
                    Inky.Move(dt);
                    Clyde.Move(dt);
                }

                Hud.Draw(Window, level);
                Map.Draw(Window);
                Player.Draw(Window);
                Blinky.Draw(Window);
                Pinky.Draw(Window);
                Inky.Draw(Window);
                Clyde.Draw(Window);

                if(QuitGameScreen)
                    MessageScreen.Draw(Window, "Are You sure You want to quit?", "y/n");
                else if(IsPaused)
                    MessageScreen.Draw(Window, "Paused");
                
                Window.Display();

                if(Player.IsDead)
                {
                    if(Player.Lives > 0)
                    {
                        Player.Respawn();
                        Blinky.Respawn();
                        Pinky.Respawn();
                        Inky.Respawn();
                        Clyde.Respawn();

                        if(Player.Lives > 1)
                            DisplayMessage($"{Player.Lives} lives left", 1000);
                        else
                            DisplayMessage("1 life left", 1000);
                    }
                    else
                    {
                        DisplayMessage("GAME OVER", 1000);
                        Ended = true;
                    }
                }
                
                if(Map.Counter == MapData.NOfDots)
                {
                    MapData.LoadLevel(++level);
                    Map.Reset();
                    DisplayMessage($"Level {level}", 1000);
                }

                Window.SetActive(false);
                WindowMutex.ReleaseMutex();

                previousTime = Clock.ElapsedTime;
                Thread.Sleep((int)(1000.0 / Settings.MaxFPS));
            }
        }

        public static void Start(RenderWindow window)
        {
            Handlers.BlockMenuEvents = true;
            Initialize(window);

            DisplayMessage($"Level {level}", 1000);

            var GameLoopThread = new Thread(new ThreadStart(GameLoop));
            GameLoopThread.Start();

            while(!Ended && Window.IsOpen)
            {
                WindowMutex.WaitOne();

                Window.DispatchEvents();

                Window.SetActive(false);
                WindowMutex.ReleaseMutex();

                Thread.Sleep(10);
            }

            GameLoopThread.Join();
            Handlers.BlockMenuEvents = false;
        }
    }
}
