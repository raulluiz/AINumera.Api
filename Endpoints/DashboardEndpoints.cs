using UsageDashboard.Api.Contracts;
using UsageDashboard.Api.Repositories;
using UsageDashboard.Api.Services;

namespace UsageDashboard.Api.Endpoints;

public static class DashboardEndpoints
{
    private const string DefaultClientId = "client-acme";
    private const string DefaultCompanyId = "company-north";
    private const string DefaultPeriod = "month";
    private const string AllUsers = "all";

    public static IEndpointRouteBuilder MapDashboardEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/dashboard");

        group.MapGet("/filters", async (IUsageRepository repository) =>
        {
            var filters = await repository.GetFiltersAsync();
            return Results.Ok(filters);
        })
        .WithName("GetDashboardFilters")
        .WithTags("Dashboard");

        group.MapGet("", async (
            string? clientId,
            string? companyId,
            string? period,
            string? userId,
            IDashboardService service) =>
        {
            var query = BuildQuery(clientId, companyId, period, userId);
            var dashboard = await service.GetDashboardAsync(query);

            return Results.Ok(dashboard);
        })
        .WithName("GetDashboard")
        .WithTags("Dashboard");

        return app;
    }

    private static DashboardQuery BuildQuery(string? clientId, string? companyId, string? period, string? userId)
    {
        return new DashboardQuery(
            clientId ?? DefaultClientId,
            companyId ?? DefaultCompanyId,
            period ?? DefaultPeriod,
            userId ?? AllUsers);
    }
}
