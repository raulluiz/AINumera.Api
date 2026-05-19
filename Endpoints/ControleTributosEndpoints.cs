using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using UsageDashboard.Api.Contracts;
using UsageDashboard.Api.Infrastructure.Database;
using UsageDashboard.Api.Services;

namespace UsageDashboard.Api.Endpoints;

public static class ControleTributosEndpoints
{
    public static IEndpointRouteBuilder MapControleTributosEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/controle-tributos")
            .WithTags("ControleTributos");

        group.MapPost("/setup-database", async (IControleTributosDatabaseInitializer initializer,CancellationToken cancellationToken) =>
        {
            await initializer.EnsureCreatedAsync(cancellationToken);

            return Results.Ok(new DatabaseSetupResponse(
                initializer.DatabaseName,
                "controletributos",
                "Banco e tabela verificados/criados com sucesso."));
        })
        .WithName("SetupControleTributosDatabase")
        .Produces<DatabaseSetupResponse>();

        group.MapPost("/upload-txt", async (HttpRequest request, IControleTributoImportService importService, CancellationToken cancellationToken) =>
        {
            if (!request.HasFormContentType)
            {
                return Results.BadRequest(new { message = "Conteúdo inválido" });
            }

            try
            {
                var boundary = HeaderUtilities.RemoveQuotes(MediaTypeHeaderValue.Parse(request.ContentType).Boundary).Value;
                var reader = new MultipartReader(boundary, request.Body);
                var section = await reader.ReadNextSectionAsync(cancellationToken);

                while (section != null)
                {
                    var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition);
                    if (hasContentDispositionHeader && contentDisposition != null && contentDisposition.DispositionType.Equals("form-data") && !string.IsNullOrEmpty(contentDisposition.FileName.Value))
                    {
                        var fileName = contentDisposition.FileName.Value ?? contentDisposition.FileNameStar.Value ?? "uploaded.txt";
                        var result = await importService.ImportAsync(section.Body, fileName, cancellationToken);
                        return Results.Ok(result);
                    }

                    section = await reader.ReadNextSectionAsync(cancellationToken);
                }

                return Results.BadRequest(new { message = "Nenhuma linha encontrada no arquivo" });
            }
            catch (InvalidDataException exception)
            {
                return Results.BadRequest(new { message = exception.Message });
            }
        })
        .WithName("UploadControleTributosTxt")
        .Produces<ControleTributoUploadResponse>()
        .DisableAntiforgery();

        return app;
    }
}
