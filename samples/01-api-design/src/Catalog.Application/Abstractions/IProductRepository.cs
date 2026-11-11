
using Catalog.Domain.Entities;

namespace Catalog.Application.Abstractions;

public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetAllAsync(
        CancellationToken cancellationToken = default);
}
