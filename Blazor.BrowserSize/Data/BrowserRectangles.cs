namespace OctarinaCompany.Blazor.BrowserSize;

public record BrowserRectangles(Rectangle Body, Rectangle Window, Rectangle Screen)
{
    public bool IsFullScreen => Window.Width == Screen.Width && Window.Height == Screen.Height;
}
