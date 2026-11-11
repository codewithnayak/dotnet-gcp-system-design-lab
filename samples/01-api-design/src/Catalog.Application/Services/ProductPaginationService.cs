
using Catalog.Application.Abstractions;
using Catalog.Application.Models;
using Catalog.Domain.Entities;

namespace Catalog.Application.Services;

public sealed class ProductPaginationService(
    IProductRepository repository)
{
    public async Task<PagedResult<Product>> GetPageAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(page, 1);

        if (pageSize < 1 || pageSize > 100)
            throw new ArgumentOutOfRangeException(nameof(pageSize));

        var products = await repository.GetAllAsync(cancellationToken);

        var totalCount = products.Count;
        var totalPages = (int)Math.Ceiling(
            totalCount / (double)pageSize);

        var items = products
            .OrderBy(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagedResult<Product>(
            items,
            page,
            pageSize,
            totalCount,
            totalPages,
            page < totalPages,
            page > 1);
    }


    public async Task<CursorPagedResult<Product>> GetCursorPageAsync(
        int? afterId,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        if (afterId < 0)
            throw new ArgumentOutOfRangeException(nameof(afterId));

        if (pageSize is < 1 or > 100)
            throw new ArgumentOutOfRangeException(nameof(pageSize));

        var products = await repository.GetAllAsync(cancellationToken);

        var batch = products
            .Where(p => p.Id > (afterId ?? 0))
            .OrderBy(p => p.Id)
            .Take(pageSize + 1)
            .ToList();

        bool hasNextPage = batch.Count > pageSize;

        var items = batch.Take(pageSize).ToList();

        int? nextCursor = hasNextPage
            ? items[^1].Id
            : null;

        return new CursorPagedResult<Product>(
            items,
            nextCursor,
            hasNextPage);
    }

}
