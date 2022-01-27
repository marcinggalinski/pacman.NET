using SFML.Graphics;

namespace pacman.NET.Config;

public static class TextureLoader
{
    public static void LoadTextures()
    {
        LoadTileTextures();
        LoadPacmanTextures();
    }

    private static void LoadTileTextures()
    {
        Globals.Textures.Tile.Wall = LoadTexture("Wall");
        Globals.Textures.Tile.Dot = LoadTexture("Dot");
        Globals.Textures.Tile.SuperDot = LoadTexture("SuperDot");
    }

    private static void LoadPacmanTextures()
    {
        Globals.Textures.Pacman = LoadTexture("Pacman");
    }
    
    private static Texture LoadTexture(string textureName)
    {
        return new Texture($"Textures/{textureName}.png");
    }
}
