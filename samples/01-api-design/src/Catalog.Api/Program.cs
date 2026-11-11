
using Catalog.Application.Abstractions;
using Catalog.Application.Services;
using Catalog.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Application services
builder.Services.AddScoped<ProductPaginationService>();

// Infrastructure
builder.Services.AddSingleton<
    IProductRepository,
    InMemoryProductRepository>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

// Required for WebApplicationFactory integration tests
public partial class Program { }
