using Pacman.Config;
using Pacman.Entities;
using Pacman.Utilities;
using SFML.Graphics;
using SFML.Window;

namespace Pacman
{
    public static class Program
    {
        public static void Main()
        {
            System.Console.WriteLine("Starting...");
            Settings.Load();

            RenderWindow window = new RenderWindow(
                new VideoMode(Settings.Resolution.Width, Settings.Resolution.Height),
                "Pacman.NET");
            window.SetActive(false);

            Handlers.SetupMenuEvents(window);

            while(window.IsOpen)
            {
                window.Clear();

                window.DispatchEvents();
                Menu.Draw(window);

                window.Display();
            }
        }
    }
}
