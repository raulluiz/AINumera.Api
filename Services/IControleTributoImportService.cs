using UsageDashboard.Api.Contracts;

namespace UsageDashboard.Api.Services;

public interface IControleTributoImportService
{
    Task<ControleTributoUploadResponse> ImportAsync(IFormFile file, CancellationToken cancellationToken);
}
