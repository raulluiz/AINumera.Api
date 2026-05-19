namespace UsageDashboard.Api.Contracts;

public sealed record DashboardQuery(string ClientId, string CompanyId, string Period, string UserId);
