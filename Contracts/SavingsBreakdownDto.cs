namespace UsageDashboard.Api.Contracts;

public sealed record SavingsBreakdownDto(decimal Daily, decimal Monthly, decimal Yearly, decimal Period);
