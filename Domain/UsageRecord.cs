namespace UsageDashboard.Api.Domain;

public sealed record UsageRecord(
    string ClientId,
    string CompanyId,
    string UserId,
    string UserName,
    DateTime LastLogin,
    int TotalMinutes,
    int MachineMinutes,
    int Sessions,
    int EntriesProcessed,
    IReadOnlyDictionary<string, int> DailyMinutes);
