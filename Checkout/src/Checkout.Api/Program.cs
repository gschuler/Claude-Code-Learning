using Checkout.Api.Endpoints;
using Checkout.Api.Errors;
using Checkout.Application;
using Checkout.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

var app = builder.Build();

app.Services.EnsureDatabaseCreated();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();               // /openapi/v1.json
    app.MapScalarApiReference();    // /scalar – interactive API docs
}

app.MapCartEndpoints();
app.MapOrderEndpoints();

app.Run();

// Exposes Program to WebApplicationFactory in integration tests.
public partial class Program;
