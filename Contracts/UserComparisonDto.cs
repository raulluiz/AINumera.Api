namespace UsageDashboard.Api.Contracts;

public sealed record UserComparisonDto(string UserName, int ActiveMinutes, int MachineMinutes, decimal Savings);
