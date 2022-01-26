using pacman.NET.Models;
using pacman.NET.Types;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace pacman.NET.Config;

public static class MapLoader
{
    public static Map LoadMap(string mapName)
    {
        var mapData = JsonSerializer.Deserialize<MapData>(File.ReadAllText($"Maps/{mapName}.json"), new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        })!;
        var tiles = new Tile[mapData.Width, mapData.Height];

        for (var row = 0; row < mapData.Height; row++)
        {
            for (var column = 0; column < mapData.Width; column++)
            {
                var position = new Position(column, row);
                tiles[column, row] = mapData.Layout[row][column] switch
                {
                    ' ' => new Tile(position, TileType.Empty),
                    '.' => new Tile(position, TileType.Empty),
                    '*' => new Tile(position, TileType.Empty),
                    '#' => new Tile(position, TileType.Wall),
                    'd' => new Tile(position, TileType.GhostHouseDoor),
                    _ => throw new ArgumentOutOfRangeException($"{mapData.Layout[row][column]}")
                };
            }
        }

        return new Map(tiles);
    }
}
