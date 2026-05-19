using UsageDashboard.Api.Domain;

namespace UsageDashboard.Api.Repositories;

public interface IControleTributoRepository
{
    Task<int> InsertManyAsync(IReadOnlyList<ControleTributo> controleTributos, CancellationToken cancellationToken);
}
