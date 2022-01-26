using SFML.Graphics;

namespace pacman.NET;

public static class RenderWindowEventHandler
{
    public static void RegisterHandler(RenderWindow window)
    {
        window.Closed += OnClosed!;
    }
    
    private static void OnClosed(object sender, EventArgs _)
    {
        var window = sender as RenderWindow;
        window!.Close();
    }
}
