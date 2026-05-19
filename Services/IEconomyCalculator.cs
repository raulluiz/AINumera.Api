using UsageDashboard.Api.Contracts;

namespace UsageDashboard.Api.Services;

public interface IEconomyCalculator
{
    decimal CalculateSavings(int entriesProcessed, int machineMinutes);
    MachineCapacityDto BuildMachineCapacity(int entriesProcessed, int machineMinutes);
    SavingsBreakdownDto BuildSavingsBreakdown(decimal periodSavings);
}
