using System;
using Pacman.Entities;
using SFML.Graphics;
using SFML.Window;

namespace Pacman.Utilities
{
    public static class Handlers
    {
        public static bool BlockGameEvents { get; set; } = false;

        private static void OnClose(object sender, EventArgs e)
        {
            ((RenderWindow)sender).Close();
        }
        private static void OnKeyPress(object sender, KeyEventArgs e)
        {
            switch(e.Code)
            {
            case Keyboard.Key.Escape:
                ((RenderWindow)sender).Close();
                break;
            case Keyboard.Key.Down:
                Menu.ActiveEntry++;
                break;
            case Keyboard.Key.Up:
                Menu.ActiveEntry--;
                break;
            case Keyboard.Key.Enter:
                Menu.ExecuteEntry((RenderWindow)sender);
                break;
            default:
                break;
            }
        }
        private static void OnKeyPress(object sender, KeyEventArgs e, Player player)
        {
            switch(e.Code)
            {
            case Keyboard.Key.Escape:
                ((RenderWindow)sender).Close();
                break;
            case Keyboard.Key.Up:
                if(!BlockGameEvents)
                    player.Turn(Direction.Up);
                break;
            case Keyboard.Key.Down:
                if(!BlockGameEvents)
                    player.Turn(Direction.Down);
                break;
            case Keyboard.Key.Left:
                if(!BlockGameEvents)
                    player.Turn(Direction.Left);
                break;
            case Keyboard.Key.Right:
                if(!BlockGameEvents)
                    player.Turn(Direction.Right);
                break;
            default:
                break;
            }
        }
        public static void SetupGameEvents(RenderWindow window, Player player)
        {
            window.Closed += OnClose;
            window.KeyPressed += (sender, e) => OnKeyPress(sender, e, player);
        }

        public static void SetupMenuEvents(RenderWindow window)
        {
            window.Closed += OnClose;
            window.KeyPressed += OnKeyPress;
        }
    }
}
