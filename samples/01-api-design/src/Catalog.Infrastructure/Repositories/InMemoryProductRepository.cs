
using Catalog.Application.Abstractions;
using Catalog.Domain.Entities;

namespace Catalog.Infrastructure.Repositories;

public sealed class InMemoryProductRepository : IProductRepository
{
    private readonly IReadOnlyList<Product> _products;

    public InMemoryProductRepository()
    {
        _products = Enumerable.Range(1, 100)
            .Select(id => new Product
            {
                Id = id,
                Name = $"Product {id}",
                Price = 10 + id,
                CreatedAtUtc = DateTime.UtcNow.AddMinutes(-id)
            })
            .ToList();
    }

    public Task<IReadOnlyList<Product>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_products);
    }
}
