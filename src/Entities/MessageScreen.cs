using Pacman.Config;
using SFML.Graphics;
using SFML.System;

namespace Pacman.Entities
{
    public static class MessageScreen
    {
        public static void Draw(RenderWindow window, params string[] messages)
        {
            var background = new RectangleShape(new Vector2f(Settings.Resolution.Width,
                                                             Settings.Resolution.Height));
            background.FillColor = new Color(0, 0, 0, 215);

            window.Draw(background);
            float vOffset = 0;
            foreach(var message in messages)
            {
                var text = new Text(message, new Font("fonts/Emulogic.ttf"), (uint)Defines.TileSize);
                text.Position = new Vector2f((Settings.Resolution.Width - text.GetLocalBounds().Width) / 2,
                                            (Settings.Resolution.Height - text.GetLocalBounds().Height) / 2 + vOffset);
                text.FillColor = Color.White;

                vOffset += text.GetLocalBounds().Height * 3 / 2;

                window.Draw(text);
            }
        }
    }
}
