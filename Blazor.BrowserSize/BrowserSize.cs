using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace OctarinaCompany.Blazor.BrowserSize;

public class BrowserSize : IBrowserSize, IAsyncDisposable
{
    private readonly ILogger<BrowserSize> _logger;
    private readonly Lazy<Task<IJSObjectReference>> moduleTask;
    private readonly DotNetObjectReference<BrowserSize> dotNetHelper;

    private event EventHandler<BrowserRectangles>? BodySizeChanged;
    private event EventHandler<BrowserRectangles>? WindowSizeChanged;
    private event EventHandler<BrowserRectangles>? ScreenSizeChanged;
    private event EventHandler<BrowserRectangles>? FullScreenChanged;

    public BrowserRectangles Rectangles { private set; get; }

    public BrowserSize(IJSRuntime jsRuntime, ILogger<BrowserSize> logger)
    {
        _logger = logger;

        moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
           "import", "./_content/OctarinaCompany.Blazor.BrowserSize/BrowserSize.js").AsTask());

        dotNetHelper = DotNetObjectReference.Create(this);

        var rect = new Rectangle(int.MinValue, int.MinValue, int.MinValue, int.MinValue);

        Rectangles = new(rect, rect, rect);
    }

    private async Task InitBrowserResizedHandler()
    {
        try
        {
            var module = await moduleTask.Value;

            await module.InvokeAsync<string>("setViewResizeHandler", "OnViewSizeChanged", dotNetHelper);
        }
        catch (JSException ex)
        {
            _logger.LogError(ex, "Exception in SetResizedHandler.");
        }
    }

    public void AddOnBodySizeChanged(EventHandler<BrowserRectangles> handler)
    {
        BodySizeChanged += handler;

        if (GetHandlersCount() == 1)
        {
            Task.Run(async () => await InitBrowserResizedHandler());
        }
    }

    public void AddOnWindowSizeChanged(EventHandler<BrowserRectangles> handler)
    {
        WindowSizeChanged += handler;

        if (GetHandlersCount() == 1)
        {
            Task.Run(async () => await InitBrowserResizedHandler());
        }
    }


    public void AddOnScreenSizeChanged(EventHandler<BrowserRectangles> handler)
    {
        ScreenSizeChanged += handler;

        if (GetHandlersCount() == 1)
        {
            Task.Run(async () => await InitBrowserResizedHandler());
        }
    }


    public void AddOnFullScreenChanged(EventHandler<BrowserRectangles> handler)
    {
        FullScreenChanged += handler;

        if (GetHandlersCount() == 1)
        {
            Task.Run(async () => await InitBrowserResizedHandler());
        }
    }


    public void RemoveOnBodySizeChanged(EventHandler<BrowserRectangles> handler)
    {
        BodySizeChanged -= handler;
    }

    public void RemoveOnWindowSizeChanged(EventHandler<BrowserRectangles> handler)
    {
        WindowSizeChanged -= handler;
    }

    public void RemoveOnScreenSizeChanged(EventHandler<BrowserRectangles> handler)
    {
        ScreenSizeChanged -= handler;
    }

    public void RemoveOnFullScreenChanged(EventHandler<BrowserRectangles> handler)
    {
        FullScreenChanged -= handler;
    }

    private int GetHandlersCount()
    {
        int count = 0;
        
        count += BodySizeChanged?.GetInvocationList().Length ?? 0;
        
        count += WindowSizeChanged?.GetInvocationList().Length ?? 0;
        
        count += ScreenSizeChanged?.GetInvocationList().Length ?? 0;
        
        count += FullScreenChanged?.GetInvocationList().Length ?? 0;

        return count;
    }

    public async Task<Rectangle> GetElementRectangle(string selector)
    {
        try
        {
            var module = await moduleTask.Value;

            var rect = await module.InvokeAsync<Rectangle>("getElementRect", selector);

            return rect;
        }
        catch (JSException ex)
        {
            _logger.LogError(ex, "Exception in SetViewResizedHandler.");

            var rect = new Rectangle(int.MinValue, int.MinValue, int.MinValue, int.MinValue);

            return rect;
        }
    }


    public async Task<Rectangle> GetElementRectangle(ElementReference element)
    {
        try
        {
            var module = await moduleTask.Value;

            var rect = await module.InvokeAsync<Rectangle>("getElementRect", element);

            return rect;
        }
        catch (JSException ex)
        {
            _logger.LogError(ex, "Exception in SetViewResizedHandler.");

            var rect = new Rectangle(int.MinValue, int.MinValue, int.MinValue, int.MinValue);

            return rect;
        }
    }


    [JSInvokable]
    public void OnViewSizeChanged(BrowserRectangles currentRectangles)
    {
        var previousSizes = Rectangles;

        Rectangles = currentRectangles;

        if (currentRectangles == previousSizes)
        {
            return;
        }

        if (currentRectangles.Body != previousSizes.Body)
        {
            BodySizeChanged?.Invoke(this, Rectangles);
            return;
        }

        if (currentRectangles.Window != previousSizes.Window)
        {
            WindowSizeChanged?.Invoke(this, Rectangles);
            return;
        }

        if (currentRectangles.Screen != previousSizes.Screen)
        {
            ScreenSizeChanged?.Invoke(this, Rectangles);
            return;
        }

        if (currentRectangles.IsFullScreen != previousSizes.IsFullScreen)
        {
            FullScreenChanged?.Invoke(this, Rectangles);
            return;
        }
    }


    public async ValueTask DisposeAsync()
    {
        if (moduleTask.IsValueCreated)
        {
            var module = await moduleTask.Value;
            await module.DisposeAsync();
        }

        dotNetHelper.Dispose();
    }

}
