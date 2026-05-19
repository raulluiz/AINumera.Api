using UsageDashboard.Api.Contracts;
using UsageDashboard.Api.Domain;
using UsageDashboard.Api.Repositories;

namespace UsageDashboard.Api.Services;

public sealed class DashboardService(IUsageRepository repository, IEconomyCalculator economyCalculator) : IDashboardService
{
    private const int ActiveLoginWindowInDays = 7;
    private static readonly string[] WeekDays = ["Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sab"];

    public async Task<DashboardResponse> GetDashboardAsync(DashboardQuery query)
    {
        var records = await repository.GetUsageAsync(query);
        var users = records.Select(ToUserDto).ToList();
        var totalActiveMinutes = users.Sum(user => user.TotalMinutes);
        var activeUsers = users.Count(user => user.IsActive);
        var sessions = users.Sum(user => user.Sessions);
        var machineMinutes = users.Sum(user => user.MachineMinutes);
        var entriesProcessed = records.Sum(record => record.EntriesProcessed);
        var periodSavings = economyCalculator.CalculateSavings(entriesProcessed, machineMinutes);

        var summary = new DashboardSummary(
            totalActiveMinutes,
            users.Count == 0 ? 0 : (int)Math.Round(totalActiveMinutes / (decimal)users.Count),
            sessions,
            activeUsers,
            users.Count - activeUsers,
            machineMinutes,
            periodSavings,
            "Economia no período selecionado");

        return new DashboardResponse(
            summary,
            users,
            BuildDailyUsage(records),
            users.Select(user => new UserComparisonDto(user.Name, user.TotalMinutes, user.MachineMinutes, user.EstimatedSavings)).ToList(),
            economyCalculator.BuildMachineCapacity(entriesProcessed, machineMinutes),
            economyCalculator.BuildSavingsBreakdown(periodSavings));
    }

    private UserUsageDto ToUserDto(UsageRecord record)
    {
        var savings = economyCalculator.CalculateSavings(record.EntriesProcessed, record.MachineMinutes);
        var active = record.LastLogin >= DateTime.Today.AddDays(-ActiveLoginWindowInDays);

        return new UserUsageDto(
            record.UserId,
            record.UserName,
            record.CompanyId,
            record.LastLogin,
            record.TotalMinutes,
            (int)Math.Round(record.DailyMinutes.Values.Average()),
            record.MachineMinutes,
            record.Sessions,
            active,
            savings);
    }

    private static IReadOnlyList<DailyUsageDto> BuildDailyUsage(IReadOnlyList<UsageRecord> records)
    {
        return WeekDays.Select(day => new DailyUsageDto(
            day,
            records.Select(record => new UserDailyMinutesDto(record.UserId, record.UserName, record.DailyMinutes.GetValueOrDefault(day))).ToList()
        )).ToList();
    }
}
