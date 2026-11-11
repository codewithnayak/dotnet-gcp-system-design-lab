
namespace Catalog.Application.Models;

public sealed record CursorPagedResult<T>(
    IReadOnlyList<T> Items,
    int? NextCursor,
    bool HasNextPage);
