using UsageDashboard.Api.Contracts;

namespace UsageDashboard.Api.Services;

public interface IDashboardService
{
    Task<DashboardResponse> GetDashboardAsync(DashboardQuery query);
}
