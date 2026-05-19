namespace UsageDashboard.Api.Contracts;

public sealed record DashboardSummary(
    int TotalActiveMinutes,
    int AverageMinutesPerUser,
    int Sessions,
    int ActiveUsers,
    int InactiveUsers,
    int MachineMinutes,
    decimal EstimatedSavings,
    string SavingsScope);
