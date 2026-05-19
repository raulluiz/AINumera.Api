namespace UsageDashboard.Api.Domain;

public sealed record EconomyAssumptions(
    decimal MonthlySalary,
    decimal MonthlyHours,
    decimal ManualEntriesPerEightHours,
    int MachineEntriesPerDay)
{
    public static EconomyAssumptions Default => new(3000m, 160m, 100000m, 2430000);
}
