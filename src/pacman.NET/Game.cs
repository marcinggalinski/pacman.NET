using pacman.NET.Config;
using SFML.Graphics;
using SFML.Window;

namespace pacman.NET;

public static class Game
{
    private static readonly RenderWindow Window = new RenderWindow(new VideoMode(Globals.WindowWidth, Globals.WindowHeight), "Pacman.NET");

    public static void Main()
    {
        RenderWindowEventHandler.RegisterHandler(Window);
        
        var map = MapLoader.LoadMap("Default");

        while (Window.IsOpen)
        {
            Window.Clear();
            
            Window.DispatchEvents();
            foreach (var tile in map)
                tile.Draw(Window, default);
            
            Window.Display();
        }
    }
}
