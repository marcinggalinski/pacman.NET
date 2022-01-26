using pacman.NET.Models;
using pacman.NET.Types;
using SFML.Graphics;
using SFML.Window;

namespace pacman.NET;

public static class Game
{
    private static readonly RenderWindow Window = new RenderWindow(new VideoMode(800, 600), "Pacman.NET");

    public static void Main()
    {
        RenderWindowEventHandler.RegisterHandler(Window);
        var tiles = new[]
        {
            new Tile(new Position(2, 2)),
            new Tile(new Position(3, 2)),
            new Tile(new Position(2, 3)),
        };
        
        while (Window.IsOpen)
        {
            Window.Clear();
            
            Window.DispatchEvents();
            foreach (var tile in tiles)
                tile.Draw(Window, default);
            
            Window.Display();
        }
    }
}
