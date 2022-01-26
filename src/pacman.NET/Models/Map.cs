using System.Collections;
using System.Collections.Generic;

namespace pacman.NET.Models;

public class Map : IEnumerable<Tile>
{
    private readonly Tile[,] _tiles;

    public Map(Tile[,] tiles)
    {
        _tiles = tiles;
        Width = (uint)tiles.GetLength(0);
        Height = (uint)tiles.GetLength(1);
    }
    
    public uint Width { get; }
    public uint Height { get; }

    public Tile this[uint column, uint row] => _tiles[column, row];

    public IEnumerator<Tile> GetEnumerator()
    {
        for (var row = 0; row < Height; row++)
            for (var column = 0; column < Width; column++)
                yield return _tiles[column, row];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
