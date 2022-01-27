using SFML.System;

namespace pacman.NET.Types;

public struct Rectangle
{
    public Rectangle(Vector2f topLeft, Vector2f topRight, Vector2f bottomRight, Vector2f bottomLeft)
    {
        TopLeft = topLeft;
        TopRight = topRight;
        BottomLeft = bottomLeft;
        BottomRight = bottomRight;
    }
    
    public Vector2f TopLeft { get; set; }
    public Vector2f TopRight { get; set; }
    public Vector2f BottomLeft { get; set; }
    public Vector2f BottomRight { get; set; }
}
