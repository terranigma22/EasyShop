using EasyShop.Data;
using EasyShop.Features.Travels;
using Microsoft.EntityFrameworkCore;
using Radzen;

namespace EasyShop;

internal static class DependencyContainer
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddMauiBlazorWebView();

        services.AddRadzen();

        services.AddTravelFeatures();

        return services;
    }

    public static IServiceCollection AddLocalDataBase(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(connectionString));

        return services;
    }

    static IServiceCollection AddTravelFeatures(this IServiceCollection services)
    {
        services.AddScoped<CreateTravelHandler>();
        services.AddScoped<UpdateTravelHandler>();
        services.AddScoped<DeleteTravelHandler>();
        services.AddScoped<GetTravelsHandler>();
        services.AddScoped<GetTravelHandler>();

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
