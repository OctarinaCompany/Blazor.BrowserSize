using Microsoft.AspNetCore.Components;

namespace OctarinaCompany.Blazor.BrowserSize;

public interface IBrowserSize
{
    void AddOnBodySizeChanged(EventHandler<BrowserRectangles> handler);
    void AddOnWindowSizeChanged(EventHandler<BrowserRectangles> handler);
    void AddOnScreenSizeChanged(EventHandler<BrowserRectangles> handler);
    void AddOnFullScreenChanged(EventHandler<BrowserRectangles> handler);

    void RemoveOnBodySizeChanged(EventHandler<BrowserRectangles> handler);
    void RemoveOnWindowSizeChanged(EventHandler<BrowserRectangles> handler);
    void RemoveOnScreenSizeChanged(EventHandler<BrowserRectangles> handler);
    void RemoveOnFullScreenChanged(EventHandler<BrowserRectangles> handler);

    Task<Rectangle> GetElementRectangle(string selector);
    Task<Rectangle> GetElementRectangle(ElementReference element);

    BrowserRectangles Rectangles { get; }

}
