using Pacman.Config;
using SFML.Graphics;
using SFML.System;

namespace Pacman.Entities
{
    public static class MessageScreen
    {
        public static void Draw(RenderWindow window, string message)
        {
            var background = new RectangleShape(new Vector2f(Settings.Resolution.Width,
                                                             Settings.Resolution.Height));
            background.FillColor = new Color(0, 0, 0, 215);

            var text = new Text(message, new Font("fonts/Emulogic.ttf"), (uint)Defines.TileSize);
            text.Position = new Vector2f((Settings.Resolution.Width - text.GetLocalBounds().Width) / 2,
                                         (Settings.Resolution.Height - text.GetLocalBounds().Height) / 2);
            text.FillColor = Color.White;

            window.Draw(background);
            window.Draw(text);
        }
    }
}
