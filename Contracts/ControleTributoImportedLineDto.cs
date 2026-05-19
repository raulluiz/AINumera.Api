namespace UsageDashboard.Api.Contracts;

public sealed record ControleTributoImportedLineDto(
    int LineNumber,
    string CodigoContabil,
    int ColumnCount,
    string Historico);
