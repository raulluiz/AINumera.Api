namespace UsageDashboard.Api.Infrastructure.Database;

public interface IControleTributosDatabaseInitializer
{
    string DatabaseName { get; }
    Task EnsureCreatedAsync(CancellationToken cancellationToken);
}
