using MySqlConnector;

namespace UsageDashboard.Api.Infrastructure.Database;

public sealed class MySqlControleTributosDatabaseInitializer(IConfiguration configuration)
    : IControleTributosDatabaseInitializer
{
    private readonly string connectionString = configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' nao configurada.");

    public string DatabaseName
    {
        get
        {
            var builder = new MySqlConnectionStringBuilder(connectionString);
            return builder.Database;
        }
    }

    public async Task EnsureCreatedAsync(CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(DatabaseName))
        {
            throw new InvalidOperationException("Informe o nome do database na connection string DefaultConnection.");
        }

        await CreateDatabaseAsync(cancellationToken);
        await CreateControleTributosTableAsync(cancellationToken);
    }

    private async Task CreateDatabaseAsync(CancellationToken cancellationToken)
    {
        var builder = new MySqlConnectionStringBuilder(connectionString)
        {
            Database = string.Empty
        };

        await using var connection = new MySqlConnection(builder.ConnectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();
        command.CommandText = $"CREATE DATABASE IF NOT EXISTS {EscapeIdentifier(DatabaseName)} CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;";
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private async Task CreateControleTributosTableAsync(CancellationToken cancellationToken)
    {
        await using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();
        command.CommandText = ControleTributosSchema.CreateControleTributosTableSql;
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private static string EscapeIdentifier(string value)
    {
        return $"`{value.Replace("`", "``")}`";
    }
}
