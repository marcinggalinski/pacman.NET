using pacman.NET.Types;
using SFML.Graphics;

namespace pacman.NET.Models;

public abstract class ModelBase : Drawable
{
    public Position Position { get; set; }
    
    protected Sprite? Sprite { get; set; }

    public void Draw(RenderTarget target, RenderStates states)
    {
        if (Sprite is not null)
            target.Draw(Sprite);
    }
}
