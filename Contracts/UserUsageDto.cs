namespace UsageDashboard.Api.Contracts;

public sealed record UserUsageDto(
    string Id,
    string Name,
    string CompanyId,
    DateTime LastLogin,
    int TotalMinutes,
    int AverageMinutesPerDay,
    int MachineMinutes,
    int Sessions,
    bool IsActive,
    decimal EstimatedSavings);
