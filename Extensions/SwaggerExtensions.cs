using Microsoft.OpenApi.Models;

namespace UsageDashboard.Api.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Usage Dashboard API",
                Version = "v1",
                Description = "API para consulta dos indicadores de uso da plataforma por cliente, empresa e usuário."
            });
        });

        return services;
    }

    public static WebApplication UseSwaggerDocumentation(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            return app;
        }

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Usage Dashboard API v1");
            options.RoutePrefix = "swagger";
            options.DocumentTitle = "Usage Dashboard API";
        });

        return app;
    }
}
