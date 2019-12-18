using System;
using Pacman.Entities;
using SFML.Graphics;
using SFML.Window;

namespace Pacman.Utilities
{
    public static class Handlers
    {
        private static void OnClose(object sender, EventArgs e)
        {
            ((RenderWindow)sender).Close();
        }
        private static void OnKeyPress(object sender, KeyEventArgs e, Player player)
        {
            switch(e.Code)
            {
            case Keyboard.Key.Escape:
                ((RenderWindow)sender).Close();
                break;
            case Keyboard.Key.Up:
                player.Turn(Direction.Up);
                break;
            case Keyboard.Key.Down:
                player.Turn(Direction.Down);
                break;
            case Keyboard.Key.Left:
                player.Turn(Direction.Left);
                break;
            case Keyboard.Key.Right:
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
    }
}
