using pacman.NET.Config;
using SFML.Graphics;
using SFML.Window;
using System;

namespace pacman.NET;

public static class RenderWindowEventHandler
{
    public static void RegisterHandlers(RenderWindow window)
    {
        window.Closed += OnClosed!;
        window.KeyPressed += OnKeyPressed!;
    }
    
    private static void OnClosed(object sender, EventArgs _)
    {
        var window = sender as RenderWindow;
        window!.Close();
    }

    private static void OnKeyPressed(object sender, KeyEventArgs args)
    {
        switch (args.Code)
        {
            case Keyboard.Key.Escape:
                OnClosed(sender, args);
                return;
        }
    }
}
