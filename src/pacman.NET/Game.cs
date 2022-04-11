using pacman.NET.Config;
using pacman.NET.Models.Actors;
using pacman.NET.Types;
using SFML.Graphics;
using SFML.Window;

namespace pacman.NET;

public static class Game
{
    private static readonly RenderWindow Window = new RenderWindow(new VideoMode(Globals.WindowWidth, Globals.WindowHeight), "Pacman.NET");

    public static void Main()
    {
        TextureLoader.LoadTextures();
        
        var map = MapLoader.LoadMap("Default");
        var pacman = new Pacman(map, new Position(1, 1));
        
        RenderWindowEventHandler.RegisterHandlers(Window, pacman);

        while (Window.IsOpen)
        {
            Window.Clear();
            
            Window.DispatchEvents();
            foreach (var tile in map)
                tile.Draw(Window, default);
            
            pacman.Move();
            pacman.Draw(Window, default);
            
            Window.Display();
        }
    }
}
