using UsageDashboard.Api.Contracts;
using UsageDashboard.Api.Domain;

namespace UsageDashboard.Api.Repositories;

public sealed class MockUsageRepository : IUsageRepository
{
    private static readonly IReadOnlyList<UsageRecord> Records =
    [
        new("client-acme", "company-north", "u-fernando", "Fernando Lopes", DateTime.Today.AddHours(-6).AddMinutes(-15), 1135, 510, 20, 820000,
            new Dictionary<string, int> { ["Dom"] = 360, ["Seg"] = 300, ["Ter"] = 480, ["Qua"] = 480, ["Qui"] = 660, ["Sex"] = 660, ["Sab"] = 720 }),
        new("client-acme", "company-north", "u-lorena", "Lorena Lima", DateTime.Today.AddHours(-8).AddMinutes(-40), 989, 440, 17, 705000,
            new Dictionary<string, int> { ["Dom"] = 260, ["Seg"] = 320, ["Ter"] = 430, ["Qua"] = 430, ["Qui"] = 540, ["Sex"] = 540, ["Sab"] = 480 }),
        new("client-acme", "company-north", "u-gustavo", "Gustavo Pereira", DateTime.Today.AddHours(-3).AddMinutes(-5), 935, 315, 15, 598000,
            new Dictionary<string, int> { ["Dom"] = 200, ["Seg"] = 270, ["Ter"] = 390, ["Qua"] = 390, ["Qui"] = 480, ["Sex"] = 480, ["Sab"] = 300 }),
        new("client-acme", "company-north", "u-marcia", "Marcia Costa", DateTime.Today.AddDays(-5).AddHours(-4), 910, 360, 13, 470000,
            new Dictionary<string, int> { ["Dom"] = 300, ["Seg"] = 240, ["Ter"] = 360, ["Qua"] = 360, ["Qui"] = 420, ["Sex"] = 420, ["Sab"] = 390 }),
        new("client-acme", "company-south", "u-renata", "Renata Alves", DateTime.Today.AddHours(-2), 760, 280, 11, 380000,
            new Dictionary<string, int> { ["Dom"] = 180, ["Seg"] = 210, ["Ter"] = 290, ["Qua"] = 330, ["Qui"] = 350, ["Sex"] = 380, ["Sab"] = 270 }),
        new("client-globo", "company-east", "u-caio", "Caio Martins", DateTime.Today.AddDays(-9), 210, 95, 4, 96000,
            new Dictionary<string, int> { ["Dom"] = 0, ["Seg"] = 40, ["Ter"] = 30, ["Qua"] = 55, ["Qui"] = 20, ["Sex"] = 65, ["Sab"] = 0 })
    ];

    public Task<FilterResponse> GetFiltersAsync()
    {
        var response = new FilterResponse(
            [new("client-acme", "Cliente Acme"), new("client-globo", "Cliente Globo")],
            [new("company-north", "Empresa Norte"), new("company-south", "Empresa Sul"), new("company-east", "Empresa Leste")],
            Records.Select(record => new FilterOption(record.UserId, record.UserName)).DistinctBy(user => user.Id).ToList(),
            [new("week", "7 dias"), new("month", "30 dias"), new("quarter", "90 dias")]);

        return Task.FromResult(response);
    }

    public Task<IReadOnlyList<UsageRecord>> GetUsageAsync(DashboardQuery query)
    {
        var result = Records
            .Where(record => record.ClientId == query.ClientId)
            .Where(record => record.CompanyId == query.CompanyId)
            .Where(record => query.UserId == "all" || record.UserId == query.UserId)
            .ToList();

        return Task.FromResult<IReadOnlyList<UsageRecord>>(result);
    }
}
