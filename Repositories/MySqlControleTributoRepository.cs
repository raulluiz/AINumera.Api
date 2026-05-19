using MySqlConnector;
using UsageDashboard.Api.Domain;

namespace UsageDashboard.Api.Repositories;

public sealed class MySqlControleTributoRepository(IConfiguration configuration) : IControleTributoRepository
{
    private const string InsertSql = """
        INSERT INTO controletributos
        (
            codigocontabil,
            DescricaoCodigoContabil,
            Historico,
            DataCadastro,
            Ativo,
            Origem,
            ChaveVinculo,
            Lancamento,
            Regra,
            TipoRegra,
            pontos
        )
        VALUES
        (
            @CodigoContabil,
            @DescricaoCodigoContabil,
            @Historico,
            @DataCadastro,
            @Ativo,
            @Origem,
            @ChaveVinculo,
            @Lancamento,
            @Regra,
            @TipoRegra,
            @Pontos
        );
        """;

    private readonly string connectionString = configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' nao configurada.");

    public async Task<int> InsertManyAsync(IReadOnlyList<ControleTributo> controleTributos, CancellationToken cancellationToken)
    {
        if (controleTributos.Count == 0)
        {
            return 0;
        }

        await using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);
        var affectedRows = 0;

        foreach (var controleTributo in controleTributos)
        {
            await using var command = new MySqlCommand(InsertSql, connection, transaction);
            AddParameter(command, "@CodigoContabil", controleTributo.CodigoContabil);
            AddParameter(command, "@DescricaoCodigoContabil", controleTributo.DescricaoCodigoContabil);
            AddParameter(command, "@Historico", controleTributo.Historico);
            AddParameter(command, "@DataCadastro", controleTributo.DataCadastro);
            AddParameter(command, "@Ativo", controleTributo.Ativo);
            AddParameter(command, "@Origem", controleTributo.Origem);
            AddParameter(command, "@ChaveVinculo", controleTributo.ChaveVinculo);
            AddParameter(command, "@Lancamento", controleTributo.Lancamento);
            AddParameter(command, "@Regra", controleTributo.Regra);
            AddParameter(command, "@TipoRegra", controleTributo.TipoRegra);
            AddParameter(command, "@Pontos", controleTributo.Pontos);

            affectedRows += await command.ExecuteNonQueryAsync(cancellationToken);
        }

        await transaction.CommitAsync(cancellationToken);

        return affectedRows;
    }

    private static void AddParameter(MySqlCommand command, string name, object? value)
    {
        command.Parameters.AddWithValue(name, value ?? DBNull.Value);
    }
}
