using EasyShop.Data;
using Microsoft.EntityFrameworkCore;
using Radzen;

namespace EasyShop;

internal static class DependencyContainer
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddMauiBlazorWebView();

        services.AddRadzen();

        return services;
    }

    public static IServiceCollection AddLocalDataBase(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(connectionString));

        return services;
    }

    static IServiceCollection AddRadzen(this IServiceCollection services)
    {
        services.AddScoped<DialogService>();
        services.AddScoped<NotificationService>();
        services.AddScoped<TooltipService>();
        services.AddScoped<ContextMenuService>();
        services.AddScoped<ThemeService>();

        services.AddRadzenCookieThemeService();
        services.AddRadzenComponents();

        return services;
    }
}
