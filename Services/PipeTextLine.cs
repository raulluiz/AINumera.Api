namespace UsageDashboard.Api.Services;

public sealed record PipeTextLine(
    int LineNumber,
    string CodigoContabil,
    string? FirstValue,
    int ColumnCount,
    string OriginalLine)
{
    public static PipeTextLine Parse(string line, int lineNumber)
    {
        var columns = line
            .Split('|', StringSplitOptions.None)
            .SkipWhile(string.IsNullOrEmpty)
            .Reverse()
            .SkipWhile(string.IsNullOrEmpty)
            .Reverse()
            .ToArray();

        if (columns.Length == 0)
        {
            throw new InvalidDataException($"Linha {lineNumber} nao possui colunas validas.");
        }

        return new PipeTextLine(
            lineNumber,
            columns[0],
            columns.ElementAtOrDefault(1),
            columns.Length,
            line);
    }
}
