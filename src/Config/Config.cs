using pacman.Entities;

namespace pacman.Config
{
    public static class Defines
    {
        public static double TopMargin { get; set; }
        public static double SideMargin { get; set; }
        public static double HudMargin { get; set; }
        public static double TileSize { get; set; }
    }

    public static class MapData
    {
        public static int Width { get; set; }
        public static int Height { get; set; }
        public static Tile[,] Tiles { get; set; }
    }
}
