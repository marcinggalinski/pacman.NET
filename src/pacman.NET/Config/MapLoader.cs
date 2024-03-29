﻿using pacman.NET.Models;
using pacman.NET.Types;
using System;
using System.IO;
using System.Text.Json;

namespace pacman.NET.Config;

public static class MapLoader
{
    public static Map LoadMap(string mapName)
    {
        var mapData = JsonSerializer.Deserialize<MapData>(File.ReadAllText($"Maps/{mapName}.json"), new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        })!;
        
        SetGlobals(mapData);
        
        var tiles = new Tile[mapData.Width, mapData.Height];
        for (uint row = 0; row < mapData.Height; row++)
        {
            for (uint column = 0; column < mapData.Width; column++)
            {
                var position = new Position(column, row);
                tiles[column, row] = mapData.Layout[row][(int)column] switch
                {
                    ' ' => new Tile(position, TileType.Accessible, TileContent.None),
                    '.' => new Tile(position, TileType.Accessible, TileContent.Dot),
                    '*' => new Tile(position, TileType.Accessible, TileContent.SuperDot),
                    '#' => new Tile(position, TileType.Wall, TileContent.None),
                    'd' => new Tile(position, TileType.GhostHouseDoor, TileContent.None),
                    _ => throw new ArgumentOutOfRangeException($"{mapData.Layout[row][(int)column]}")
                };
            }
        }

        return new Map(tiles);
    }

    private static void SetGlobals(MapData mapData)
    {
        var tileSizeFromWidth = Globals.WindowWidth / mapData.Width;
        var tileSizeFromHeight = Globals.WindowHeight / mapData.Height;

        if (tileSizeFromWidth < tileSizeFromHeight)
        {
            Globals.TileSize = tileSizeFromWidth % 2 == 0 ? tileSizeFromWidth : tileSizeFromWidth - 1;
            Globals.TopMargin = (Globals.WindowHeight - mapData.Height * Globals.TileSize) / 2;
            Globals.LeftMargin = (Globals.WindowWidth - mapData.Width * Globals.TileSize) / 2;
        }
        else
        {
            Globals.TileSize = tileSizeFromHeight % 2 == 0 ? tileSizeFromHeight : tileSizeFromHeight - 1;
            Globals.TopMargin = (Globals.WindowHeight - mapData.Height * Globals.TileSize) / 2;
            Globals.LeftMargin = (Globals.WindowWidth - mapData.Width * Globals.TileSize) / 2;
        }
        
        Globals.MoveUnit = Globals.TileSize / 20f;
    }
}
