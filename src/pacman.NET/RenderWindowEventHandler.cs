using pacman.NET.Models;
using pacman.NET.Types;
using SFML.Graphics;
using SFML.Window;
using System;

namespace pacman.NET;

public static class RenderWindowEventHandler
{
    public static void RegisterHandlers(RenderWindow window, Pacman pacman)
    {
        window.Closed += (sender, _) => OnClosed((sender as RenderWindow)!);
        window.KeyPressed += (sender, args) => OnKeyPressed((sender as RenderWindow)!, args, pacman);
    }
    
    private static void OnClosed(RenderWindow window)
    {
        window.Close();
    }

    private static void OnKeyPressed(RenderWindow window, KeyEventArgs args, Pacman pacman)
    {
        switch (args.Code)
        {
            case Keyboard.Key.Escape:
                OnClosed(window);
                return;
            case Keyboard.Key.Left:
                pacman.PlanTurn(Direction.Left);
                break;
            case Keyboard.Key.Up:
                pacman.PlanTurn(Direction.Up);
                break;
            case Keyboard.Key.Right:
                pacman.PlanTurn(Direction.Right);
                break;
            case Keyboard.Key.Down:
                pacman.PlanTurn(Direction.Down);
                break;
        }
    }
}
