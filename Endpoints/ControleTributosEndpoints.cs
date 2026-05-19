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

        group.MapPost("/setup-database", async (
            IControleTributosDatabaseInitializer initializer,
            CancellationToken cancellationToken) =>
        {
            await initializer.EnsureCreatedAsync(cancellationToken);

            return Results.Ok(new DatabaseSetupResponse(
                initializer.DatabaseName,
                "controletributos",
                "Banco e tabela verificados/criados com sucesso."));
        })
        .WithName("SetupControleTributosDatabase")
        .Produces<DatabaseSetupResponse>();

        group.MapPost("/upload-txt", async (
            IFormFile file,
            IControleTributoImportService importService,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await importService.ImportAsync(file, cancellationToken);
                return Results.Ok(result);
            }
            catch (InvalidDataException exception)
            {
                return Results.BadRequest(new { message = exception.Message });
            }
        })
        .WithName("UploadControleTributosTxt")
        .Accepts<IFormFile>("multipart/form-data")
        .Produces<ControleTributoUploadResponse>()
        .DisableAntiforgery();

        return app;
    }
}
