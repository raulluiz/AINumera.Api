namespace UsageDashboard.Api.Contracts;

public sealed record ControleTributoUploadResponse(
    string FileName,
    int TotalLinesRead,
    int ParsedRecords,
    int InsertedRecords,
    double TotalSeconds,
    double DatabaseSeconds,
    IReadOnlyList<ControleTributoImportedLineDto> Preview);
