using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using UsageDashboard.Api.Endpoints;
using UsageDashboard.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Allow large request bodies for big file uploads
builder.WebHost.UseUrls("http://localhost:5078");
builder.WebHost.ConfigureKestrel(opts =>
{
    // null = no limit
    opts.Limits.MaxRequestBodySize = null;
});

// Increase multipart form limits to accept very large files
builder.Services.Configure<FormOptions>(opts =>
{
    opts.MultipartBodyLengthLimit = long.MaxValue;
    opts.ValueCountLimit = int.MaxValue;
    opts.MultipartHeadersLengthLimit = int.MaxValue;
});

builder.Services.AddFrontendCors();
builder.Services.AddDashboardServices();
builder.Services.AddSwaggerDocumentation();

var app = builder.Build();

app.UseSwaggerDocumentation();
app.UseCors(CorsPolicies.Frontend);
app.MapDashboardEndpoints();
app.MapControleTributosEndpoints();

app.Run();
