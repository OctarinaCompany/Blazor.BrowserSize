
using Microsoft.Extensions.DependencyInjection;

namespace OctarinaCompany.Blazor.BrowserSize;

static public class BrowserSizeExtensions
{
    static public void AddBrowserSize(this IServiceCollection services)
    {
        services.AddSingleton<IBrowserSize, BrowserSize>();
    }
}
