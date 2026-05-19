using UsageDashboard.Api.Domain;
using UsageDashboard.Api.Infrastructure.Database;
using UsageDashboard.Api.Repositories;
using UsageDashboard.Api.Services;

namespace UsageDashboard.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFrontendCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicies.Frontend, policy =>
                policy.WithOrigins("http://localhost:5173", "http://127.0.0.1:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod());
        });

        return services;
    }

    public static IServiceCollection AddDashboardServices(this IServiceCollection services)
    {
        services.AddSingleton(EconomyAssumptions.Default);
        services.AddSingleton<IUsageRepository, MockUsageRepository>();
        services.AddScoped<IEconomyCalculator, EconomyCalculator>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IControleTributosDatabaseInitializer, MySqlControleTributosDatabaseInitializer>();
        services.AddScoped<IControleTributoRepository, MySqlControleTributoRepository>();
        services.AddScoped<IControleTributoImportService, ControleTributoImportService>();

        return services;
    }
}
