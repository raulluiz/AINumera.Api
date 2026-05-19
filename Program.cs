using UsageDashboard.Api.Endpoints;
using UsageDashboard.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5078");

builder.Services.AddFrontendCors();
builder.Services.AddDashboardServices();

var app = builder.Build();

app.UseCors(CorsPolicies.Frontend);
app.MapDashboardEndpoints();

app.Run();
