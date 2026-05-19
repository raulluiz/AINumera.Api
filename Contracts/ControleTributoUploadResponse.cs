namespace UsageDashboard.Api.Contracts;

public sealed record ControleTributoUploadResponse(
    string FileName,
    int TotalLinesRead,
    int ParsedRecords,
    int InsertedRecords,
    long TotalElapsedMilliseconds,
    long DatabaseElapsedMilliseconds,
    IReadOnlyList<ControleTributoImportedLineDto> Preview);
