
namespace Catalog.Domain.Entities;

public sealed class Product
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public decimal Price { get; init; }
    public DateTime CreatedAtUtc { get; init; }
}
