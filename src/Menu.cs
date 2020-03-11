using Pacman.Config;
using Pacman.Utilities;
using SFML.Graphics;
using SFML.System;

namespace Pacman.Entities
{
    public static class Menu
    {
        private static int activeEntry;

        private static Sprite Background { get; set; }
        private static MenuEntry[] Entries { get; set; }
        public static int ActiveEntry
        {
            get => activeEntry;
            set
            {
                Entries[activeEntry].IsActive = false;
                activeEntry = (value + Entries.Length) % Entries.Length;
                Entries[activeEntry].IsActive = true;
            }
        }

        static Menu()
        {
            // initialize Background
            Background = new Sprite(new Texture("textures/bg.png"));
            Background.Scale = new Vector2f(
                Settings.Resolution.Width / Background.Texture.Size.X,
                Settings.Resolution.Height / Background.Texture.Size.Y);

            // initialize Entries
            Entries = new MenuEntry[3];
            Entries[0] = new MenuEntry("Play", Game.Start, Defines.TopMargin + Settings.Resolution.Height / 2);
            Entries[1] = new MenuEntry("Settings", (window) => throw new System.NotImplementedException(), Defines.TopMargin + Settings.Resolution.Height / 2 + Settings.Resolution.Height / 10);
            Entries[2] = new MenuEntry("Quit", (window) => window.Close(), Defines.TopMargin + Settings.Resolution.Height / 2 + Settings.Resolution.Height / 5);
            ActiveEntry = 0;
            Entries[ActiveEntry].IsActive = true;
        }

        public static void ExecuteEntry(RenderWindow window)
        {
            Entries[ActiveEntry].Chosen.Invoke(window);
        }
        public static void Draw(RenderWindow window)
        {
            window.Draw(Background);
            foreach(var entry in Entries)
                entry.Draw(window);
        }
    }
}
