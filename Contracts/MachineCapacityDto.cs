namespace UsageDashboard.Api.Contracts;

public sealed record MachineCapacityDto(
    int MachineEntriesPerDay,
    decimal EquivalentHumanAnalysts,
    int ManualHoursEquivalent,
    int MachineHours,
    int EntriesProcessed);
