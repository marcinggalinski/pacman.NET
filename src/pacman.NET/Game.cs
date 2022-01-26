using SFML.Graphics;
using SFML.Window;

namespace pacman.NET;

public static class Game
{
    private static readonly RenderWindow Window = new RenderWindow(new VideoMode(800, 600), "Pacman.NET");

    public static void Main()
    {
        RenderWindowEventHandler.RegisterHandler(Window);
        
        while (Window.IsOpen)
        {
            Window.Clear();
            
            Window.DispatchEvents();
            
            Window.Display();
        }
    }
}
