namespace pacman.NET.Config;

public class MapData
{
    public uint Width { get; set; }
    public uint Height { get; set; }
    public string[] Layout { get; set; } = null!;
}
