using UsageDashboard.Api.Contracts;

namespace UsageDashboard.Api.Services;

public interface IControleTributoImportService
{
    Task<ControleTributoUploadResponse> ImportAsync(IFormFile file, CancellationToken cancellationToken);
    Task<ControleTributoUploadResponse> ImportAsync(System.IO.Stream stream, string fileName, CancellationToken cancellationToken);
}
