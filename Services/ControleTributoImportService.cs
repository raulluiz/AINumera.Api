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
        var totalStopwatch = Stopwatch.StartNew();
        var databaseStopwatch = new Stopwatch();

        ValidateFile(file);

        databaseStopwatch.Start();
        await databaseInitializer.EnsureCreatedAsync(cancellationToken);
        databaseStopwatch.Stop();

        var importedLines = new List<ControleTributoImportedLineDto>();
        var records = new List<ControleTributo>();
        var totalLinesRead = 0;

        await using var stream = file.OpenReadStream();
        using var reader = new StreamReader(stream);

        while (await reader.ReadLineAsync(cancellationToken) is { } line)
        {
            totalLinesRead++;

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var parsedLine = PipeTextLine.Parse(line, totalLinesRead);
            records.Add(ToControleTributo(parsedLine));

            if (importedLines.Count < PreviewLimit)
            {
                importedLines.Add(new ControleTributoImportedLineDto(
                    parsedLine.LineNumber,
                    parsedLine.CodigoContabil,
                    parsedLine.ColumnCount,
                    parsedLine.OriginalLine));
            }
        }

        databaseStopwatch.Start();
        var insertedRecords = await repository.InsertManyAsync(records, cancellationToken);
        databaseStopwatch.Stop();
        totalStopwatch.Stop();

        return new ControleTributoUploadResponse(
            file.FileName,
            totalLinesRead,
            records.Count,
            insertedRecords,
            totalStopwatch.ElapsedMilliseconds,
            databaseStopwatch.ElapsedMilliseconds,
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
