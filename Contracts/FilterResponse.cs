namespace UsageDashboard.Api.Contracts;

public sealed record FilterResponse(
    IReadOnlyList<FilterOption> Clients,
    IReadOnlyList<FilterOption> Companies,
    IReadOnlyList<FilterOption> Users,
    IReadOnlyList<FilterOption> Periods);
