using System;
using System.IO;
using Newtonsoft.Json.Linq;
using Pacman.Entities;
using Pacman.Utilities;
using SFML.Graphics;

namespace Pacman.Config
{
    public static class Settings
    {
        public static Resolution_t Resolution;
        public static bool Fullscreen { get; set; }
        public static float MaxFPS { get; set; }

        public static void Load()
        {
            Console.WriteLine("Loading settings");
            var settings = JObject.Parse(new StreamReader("settings.json").ReadToEnd());
            Resolution.Width = settings["resolution"]["width"].ToObject<uint>();
            Resolution.Height = settings["resolution"]["height"].ToObject<uint>();
            Fullscreen = settings["fullscreen"].ToObject<bool>();
            MaxFPS = settings["maxFPS"].ToObject<float>();
        }
    }

    public static class Textures
    {
        public class GhostTextures
        {
            public Texture Normal { get; set; }
            public Texture Frightened { get; set; }
            public Texture Dead { get; set; }
        }
        public static Texture Dot { get; set; }
        public static Texture Superdot { get; set; }
        public static Texture Wall { get; set; }
        public static Texture GhosthouseDoor { get; set; }
        public static Texture Pacman { get; set; }
        public static GhostTextures Blinky { get; set; }
        public static GhostTextures Pinky { get; set; }
        public static GhostTextures Inky { get; set; }
        public static GhostTextures Clyde { get; set; }

        public static void Load()
        {
            Console.WriteLine("Loading textures");
            Dot = new Texture("textures/dot.png");
            Superdot = new Texture("textures/superdot.png");
            Wall = new Texture("textures/wall.png");
            GhosthouseDoor = new Texture("textures/door.png");
            Pacman = new Texture("textures/pacman.png");

            // blinky
            Blinky = new GhostTextures();
            Blinky.Normal = new Texture("textures/blinky/normal.png");
            Blinky.Frightened = new Texture("textures/blinky/frightened.png");
            Blinky.Dead = new Texture("textures/blinky/dead.png");
            // pinky
            Pinky = new GhostTextures();
            Pinky.Normal = new Texture("textures/pinky/normal.png");
            Pinky.Frightened = new Texture("textures/pinky/frightened.png");
            Pinky.Dead = new Texture("textures/pinky/dead.png");
            // inky
            Inky = new GhostTextures();
            Inky.Normal = new Texture("textures/inky/normal.png");
            Inky.Frightened = new Texture("textures/inky/frightened.png");
            Inky.Dead = new Texture("textures/inky/dead.png");
            // clyde
            Clyde = new GhostTextures();
            Clyde.Normal = new Texture("textures/clyde/normal.png");
            Clyde.Frightened = new Texture("textures/clyde/frightened.png");
            Clyde.Dead = new Texture("textures/clyde/dead.png");
        }
    }

    public static class Defines
    {
        public static float TopMargin { get; set; }
        public static float SideMargin { get; set; }
        public static float HudMargin { get; set; }
        public static float TileSize { get; set; }
        public static float BaseSpeed { get; set; }
    }

    public static class MapData
    {
        public static int Width { get; set; }
        public static int Height { get; set; }
        public static Tile[,] Tiles { get; set; }
        public static int[,] IntMap { get; set; }
        public static Coords_t Pacman { get; set; }

        public static void Load()
        {
            Console.WriteLine("Loading map");

            // reading and parsing map file
            string mapName = JObject.Parse(new StreamReader("settings.json").ReadToEnd())["map"].ToString();
            Console.WriteLine(mapName);
            var map = JObject.Parse(new StreamReader("maps/" + mapName + ".pmmap").ReadToEnd());

            // reading and pre-setting map properites
            Width = map["width"].ToObject<int>();
            Height = map["height"].ToObject<int>();
            Defines.BaseSpeed = map["baseSpeed"].ToObject<float>();
            Tiles = new Tile[Width, Height];
            IntMap = new int[Width, Height];

            // calculating tile size
            float tileSize = (float)Settings.Resolution.Width / (float)Width;
            if(tileSize > Settings.Resolution.Height / (Height + 20.0f/7.0f))
                tileSize = Settings.Resolution.Height / (Height + 20.0f/7.0f);
            
            Defines.TileSize = tileSize;

            // setting hud margin
            Defines.HudMargin = (int)(tileSize * 20.0 / 7.0);

            // calculating side margin
            Defines.SideMargin = (float)(Settings.Resolution.Width - tileSize * Width) / 2;

            // calculating top margin
            Defines.TopMargin = (float)(Settings.Resolution.Height - Defines.HudMargin - tileSize * Height) / 2;

            // reading actors properties
            Pacman = new Coords_t(
                map["pacman"]["x"].ToObject<float>() * Defines.TileSize + Defines.SideMargin,
                map["pacman"]["y"].ToObject<float>() * Defines.TileSize + Defines.TopMargin + Defines.HudMargin);

            // reading tiles
            for(int r = 0; r < Height; r++)
            {
                for(int c = 0; c < Width; c++)
                {
                    switch((map["tiles"][r].ToString())[c])
                    {
                    case '.':
                        Tiles[c, r] = new Tile(new Position_t(c, r), TileContent.Dot, Textures.Dot);
                        IntMap[c, r] = 1;
                        break;
                    case '*':
                        Tiles[c, r] = new Tile(new Position_t(c, r), TileContent.SuperDot, Textures.Superdot);
                        IntMap[c, r] = 1;
                        break;
                    case '#':
                        Tiles[c, r] = new Tile(new Position_t(c, r), TileContent.Wall, Textures.Wall);
                        IntMap[c, r] = 0;
                        break;
                    case 'd':
                        Tiles[c, r] = new Tile(new Position_t(c, r), TileContent.GhosthouseDoor, Textures.GhosthouseDoor);
                        IntMap[c, r] = 1;
                        break;
                    case 'g':
                        Tiles[c, r] = new Tile(new Position_t(c, r), TileContent.Ghosthouse);
                        IntMap[c, r] = 1;
                        break;
                    case 'o':
                        Tiles[c, r] = new Tile(new Position_t(c, r), TileContent.None);
                        IntMap[c, r] = 0;
                        break;
                    case 't':
                        Tiles[c, r] = new Tile(new Position_t(c, r), TileContent.Tunel);
                        IntMap[c, r] = 1;
                        break;
                    case ' ':
                        Tiles[c, r] = new Tile(new Position_t(c, r), TileContent.None);
                        IntMap[c, r] = 1;
                        break;
                    }
                }
            }
        }
    }
}
