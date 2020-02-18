using SFML.Graphics;
using SFML.System;

namespace Pacman.Entities
{
    public class Hud
    {
        private Font font = new Font("fonts/Emulogic.ttf");
        private Text score = new Text();
        private Text lives = new Text();
        private Player player;

        public Hud(Player _player)
        {
            player = _player;

            var charSize = (uint)Config.Defines.HudMargin / 2;

            score.Font = font;
            score.FillColor = Color.White;
            score.CharacterSize = charSize;
            score.Position = new Vector2f(Config.Defines.SideMargin, Config.Defines.TopMargin);

            lives.Font = font;
            lives.FillColor = Color.White;
            lives.CharacterSize = charSize;
            lives.Position = new Vector2f(Config.Defines.SideMargin, Config.Defines.TopMargin + charSize);
        }

        public void Draw(RenderWindow window)
        {
            score.DisplayedString = $"Score: {player.Score}";
            lives.DisplayedString = $"Lives left: {player.Lives}";

            window.Draw(score);
            window.Draw(lives);
        }
    }
}
