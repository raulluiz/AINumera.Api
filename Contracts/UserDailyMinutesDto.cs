namespace UsageDashboard.Api.Contracts;

public sealed record UserDailyMinutesDto(string UserId, string UserName, int Minutes);
