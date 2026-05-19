using UsageDashboard.Api.Endpoints;
using UsageDashboard.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5078");

builder.Services.AddFrontendCors();
builder.Services.AddDashboardServices();
builder.Services.AddSwaggerDocumentation();

var app = builder.Build();

app.UseSwaggerDocumentation();
app.UseCors(CorsPolicies.Frontend);
app.MapDashboardEndpoints();

app.Run();
