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
        // Build a single multi-row INSERT to reduce round-trips.
        await using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

        var sb = new System.Text.StringBuilder();
        sb.Append("INSERT INTO controletributos (codigocontabil, DescricaoCodigoContabil, Historico, DataCadastro, Ativo, Origem, ChaveVinculo, Lancamento, Regra, TipoRegra, pontos) VALUES ");

        var parameters = new List<MySqlParameter>(controleTributos.Count * 11);

        for (var i = 0; i < controleTributos.Count; i++)
        {
            var prefix = i.ToString();
            sb.Append('(');
            sb.Append($"@CodigoContabil{prefix},@DescricaoCodigoContabil{prefix},@Historico{prefix},@DataCadastro{prefix},@Ativo{prefix},@Origem{prefix},@ChaveVinculo{prefix},@Lancamento{prefix},@Regra{prefix},@TipoRegra{prefix},@Pontos{prefix}");
            sb.Append(')');
            if (i < controleTributos.Count - 1)
                sb.Append(',');

            var c = controleTributos[i];
            parameters.Add(new MySqlParameter($"@CodigoContabil{prefix}", c.CodigoContabil ?? (object)DBNull.Value));
            parameters.Add(new MySqlParameter($"@DescricaoCodigoContabil{prefix}", c.DescricaoCodigoContabil ?? (object)DBNull.Value));
            parameters.Add(new MySqlParameter($"@Historico{prefix}", c.Historico ?? (object)DBNull.Value));
            parameters.Add(new MySqlParameter($"@DataCadastro{prefix}", c.DataCadastro));
            parameters.Add(new MySqlParameter($"@Ativo{prefix}", c.Ativo));
            parameters.Add(new MySqlParameter($"@Origem{prefix}", c.Origem));
            parameters.Add(new MySqlParameter($"@ChaveVinculo{prefix}", c.ChaveVinculo ?? (object)DBNull.Value));
            parameters.Add(new MySqlParameter($"@Lancamento{prefix}", c.Lancamento ?? (object)DBNull.Value));
            parameters.Add(new MySqlParameter($"@Regra{prefix}", c.Regra ?? (object)DBNull.Value));
            parameters.Add(new MySqlParameter($"@TipoRegra{prefix}", c.TipoRegra ?? (object)DBNull.Value));
            parameters.Add(new MySqlParameter($"@Pontos{prefix}", c.Pontos));
        }

        await using var command = new MySqlCommand(sb.ToString(), connection, transaction);
        command.Parameters.AddRange(parameters.ToArray());
        var affectedRows = await command.ExecuteNonQueryAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return affectedRows;
    }

    private static void AddParameter(MySqlCommand command, string name, object? value)
    {
        command.Parameters.AddWithValue(name, value ?? DBNull.Value);
    }
}
