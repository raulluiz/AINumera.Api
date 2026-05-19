using UsageDashboard.Api.Contracts;
using UsageDashboard.Api.Domain;

namespace UsageDashboard.Api.Repositories;

public interface IUsageRepository
{
    Task<FilterResponse> GetFiltersAsync();
    Task<IReadOnlyList<UsageRecord>> GetUsageAsync(DashboardQuery query);
}
