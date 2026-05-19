namespace UsageDashboard.Api.Contracts;

public sealed record DailyUsageDto(string Day, IReadOnlyList<UserDailyMinutesDto> Users);
