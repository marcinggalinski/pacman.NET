using SFML.Graphics;

namespace pacman.NET.Config;

public static class TextureLoader
{
    public static void LoadTextures()
    {
        Globals.Textures.Tile.Wall = LoadTexture("Wall");
        Globals.Textures.Tile.Dot = LoadTexture("Dot");
        Globals.Textures.Tile.SuperDot = LoadTexture("SuperDot");
    }
    
    private static Texture LoadTexture(string textureName)
    {
        return new Texture($"Textures/{textureName}.png");
    }
}
