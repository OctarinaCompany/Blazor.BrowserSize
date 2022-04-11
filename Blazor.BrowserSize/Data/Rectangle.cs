namespace Octarina.BrowserSize;

public record Rectangle(int X, int Y, int Width, int Height)
{
    public int Left => X;
    public int Right => X + Width;
    public int Top => Y;
    public int Bottom => Y + Height;
};
