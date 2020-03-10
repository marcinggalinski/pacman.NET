using System;
using Pacman.Config;
using Pacman.Utilities;
using SFML.Graphics;
using SFML.System;

namespace Pacman.Entities
{
    public class MenuEntry
    {
        private bool isActive;

        private Font Font { get; set; }
        public Text Text { get; set; }
        public Action<RenderWindow> Chosen { get; private set; }
        public bool IsActive
        {
            get => isActive;
            set
            {
                isActive = value;
                Text.FillColor = isActive ? Color.Yellow : Color.White;
            }
        }

        public MenuEntry(string text, Action<RenderWindow> onChoose, float y)
        {
            Font = new Font("fonts/Emulogic.ttf");
            Text = new Text(text, Font, Settings.Resolution.Height / 10);
            Text.Position = new Vector2f((Settings.Resolution.Width - Text.GetLocalBounds().Width) / 2, y);
            Text.FillColor = Color.White;
            IsActive = false;
            Chosen += onChoose;
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(Text);
        }
    }
}
