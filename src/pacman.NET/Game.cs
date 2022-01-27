using pacman.NET.Config;
using pacman.NET.Models;
using SFML.Graphics;
using SFML.Window;
using System.Collections.Generic;

namespace pacman.NET;

public static class Game
{
    public static List<ModelBase> Models { get; set; } = new List<ModelBase>();
    
    private static readonly RenderWindow Window = new RenderWindow(new VideoMode(Globals.WindowWidth, Globals.WindowHeight), "Pacman.NET");

    public static void Main()
    {
        RenderWindowEventHandler.RegisterHandlers(Window);
        TextureLoader.LoadTextures();
        
        var map = MapLoader.LoadMap("Default");
        foreach (var tile in map)
            Models.Add(tile);

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
