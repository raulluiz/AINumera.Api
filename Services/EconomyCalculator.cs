using UsageDashboard.Api.Contracts;
using UsageDashboard.Api.Domain;

namespace UsageDashboard.Api.Services;

public sealed class EconomyCalculator(EconomyAssumptions assumptions) : IEconomyCalculator
{
    public decimal CalculateSavings(int entriesProcessed, int machineMinutes)
    {
        var manualHours = CalculateManualHours(entriesProcessed);
        var machineHours = ConvertMinutesToHours(machineMinutes);
        var savedHours = Math.Max(0m, manualHours - machineHours);
        var hourlyCost = assumptions.MonthlySalary / assumptions.MonthlyHours;

        return Math.Round(savedHours * hourlyCost, 2);
    }

    public MachineCapacityDto BuildMachineCapacity(int entriesProcessed, int machineMinutes)
    {
        var manualHours = CalculateManualHours(entriesProcessed);
        var machineHours = ConvertMinutesToHours(machineMinutes);
        var equivalentAnalysts = manualHours / 8m;

        return new MachineCapacityDto(
            assumptions.MachineEntriesPerDay,
            Math.Round(equivalentAnalysts, 1),
            (int)Math.Round(manualHours),
            (int)Math.Round(machineHours),
            entriesProcessed);
    }

    public SavingsBreakdownDto BuildSavingsBreakdown(decimal periodSavings)
    {
        return new SavingsBreakdownDto(
            Daily: periodSavings / 30m,
            Monthly: periodSavings,
            Yearly: periodSavings * 12m,
            Period: periodSavings);
    }

    private decimal CalculateManualHours(int entriesProcessed)
    {
        return entriesProcessed / assumptions.ManualEntriesPerEightHours * 8m;
    }

    private static decimal ConvertMinutesToHours(int minutes)
    {
        return minutes / 60m;
    }
}
