namespace UsageDashboard.Api.Contracts;

public sealed record DashboardResponse(
    DashboardSummary Summary,
    IReadOnlyList<UserUsageDto> Users,
    IReadOnlyList<DailyUsageDto> DailyUsage,
    IReadOnlyList<UserComparisonDto> UserComparison,
    MachineCapacityDto MachineCapacity,
    SavingsBreakdownDto Savings);
