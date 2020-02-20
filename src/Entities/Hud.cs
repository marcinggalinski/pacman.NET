using System.Text;
using SFML.Graphics;
using SFML.System;

namespace Pacman.Entities
{
    public class Hud
    {
        private Font Font { get; set; } = new Font("fonts/Emulogic.ttf");
        private Text Score { get; set; } = new Text();
        private Text Level { get; set; } = new Text();
        private Text Lives { get; set; } = new Text();
        private Player Player { get; set; }

        public Hud(Player player)
        {
            Player = player;

            var charSize = (uint)Config.Defines.HudMargin / 2;

            Score.Font = Font;
            Score.FillColor = Color.White;
            Score.CharacterSize = charSize;
            Score.Position = new Vector2f(Config.Defines.SideMargin, Config.Defines.TopMargin);

            Level.Font = Font;
            Level.FillColor = Color.White;
            Level.CharacterSize = charSize;
            Level.Position = new Vector2f(Config.Defines.SideMargin, Config.Defines.TopMargin + charSize);

            Lives.Font = Font;
            Lives.FillColor = Color.White;
            Lives.CharacterSize = charSize;
        }

        public void Draw(RenderWindow window, uint level)
        {
            Score.DisplayedString = $"Score: {Player.Score}";
            Level.DisplayedString = $"Level: {level}";

            var livesString = new StringBuilder();
            for(int i = 0; i < Player.Lives; i++)
                livesString.Append("<3");
            Lives.DisplayedString = livesString.ToString();
            Lives.Position = new Vector2f(Config.Settings.Resolution.Width - Config.Defines.SideMargin - Lives.GetLocalBounds().Width - Lives.CharacterSize / 3,
                                          (Config.Defines.HudMargin - Lives.GetLocalBounds().Height) / 2);

            window.Draw(Score);
            window.Draw(Level);
            window.Draw(Lives);
        }
    }
}
