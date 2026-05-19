using System.Diagnostics;
using UsageDashboard.Api.Contracts;
using UsageDashboard.Api.Domain;
using UsageDashboard.Api.Infrastructure.Database;
using UsageDashboard.Api.Repositories;

namespace UsageDashboard.Api.Services;

public sealed class ControleTributoImportService(
    IControleTributosDatabaseInitializer databaseInitializer,
    IControleTributoRepository repository) : IControleTributoImportService
{
    private const int PreviewLimit = 10;

    public async Task<ControleTributoUploadResponse> ImportAsync(IFormFile file, CancellationToken cancellationToken)
    {
        await using var stream = file.OpenReadStream();
        return await ImportAsync(stream, file.FileName, cancellationToken);
    }

    public async Task<ControleTributoUploadResponse> ImportAsync(System.IO.Stream stream, string fileName, CancellationToken cancellationToken)
    {
        var totalStopwatch = Stopwatch.StartNew();
        var databaseStopwatch = new Stopwatch();

        // Ensure database exists before processing large file
        databaseStopwatch.Start();
        await databaseInitializer.EnsureCreatedAsync(cancellationToken);
        databaseStopwatch.Stop();

        var importedLines = new List<ControleTributoImportedLineDto>();
        var batch = new List<ControleTributo>();
        var totalLinesRead = 0;
        var totalInserted = 0;
        const int BatchSize = 10000;

        using var reader = new StreamReader(stream);
        while (await reader.ReadLineAsync(cancellationToken) is { } line)
        {
            totalLinesRead++;

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var parsedLine = PipeTextLine.Parse(line, totalLinesRead);
            batch.Add(ToControleTributo(parsedLine));

            if (importedLines.Count < PreviewLimit)
            {
                importedLines.Add(new ControleTributoImportedLineDto(
                    parsedLine.LineNumber,
                    parsedLine.CodigoContabil,
                    parsedLine.ColumnCount,
                    parsedLine.OriginalLine));
            }

            if (batch.Count >= BatchSize)
            {
                databaseStopwatch.Start();
                var inserted = await repository.InsertManyAsync(batch, cancellationToken);
                databaseStopwatch.Stop();
                totalInserted += inserted;
                batch.Clear();
            }
        }

        if (batch.Count > 0)
        {
            databaseStopwatch.Start();
            var inserted = await repository.InsertManyAsync(batch, cancellationToken);
            databaseStopwatch.Stop();
            totalInserted += inserted;
            batch.Clear();
        }

        totalStopwatch.Stop();

        return new ControleTributoUploadResponse(
            fileName,
            totalLinesRead,
            totalLinesRead, 
            totalInserted,
            totalStopwatch.Elapsed.TotalSeconds,
            databaseStopwatch.Elapsed.TotalSeconds,
            importedLines);
    }

    private static void ValidateFile(IFormFile file)
    {
        if (file.Length == 0)
        {
            throw new InvalidDataException("Arquivo vazio.");
        }

        var extension = Path.GetExtension(file.FileName);
        if (!string.Equals(extension, ".txt", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidDataException("Envie um arquivo com extensao .txt.");
        }
    }

    private static ControleTributo ToControleTributo(PipeTextLine line)
    {
        return new ControleTributo
        {
            CodigoContabil = line.CodigoContabil,
            DescricaoCodigoContabil = line.FirstValue,
            Historico = line.OriginalLine,
            HistoricoOriginal = line.OriginalLine,
            DataCadastro = DateTime.Now,
            Ativo = true,
            Origem = true,
            ChaveVinculo = $"TXT-{line.LineNumber}",
            Lancamento = line.CodigoContabil,
            Regra = "IMPORTACAO_TXT",
            TipoRegra = "Arquivo separado por pipe",
            Pontos = line.ColumnCount
        };
    }
}
