
using Catalog.Application.Services;
using Catalog.Infrastructure.Repositories;
using Xunit;

namespace Catalog.UnitTests;

public class ProductPaginationServiceTests
{
    private readonly ProductPaginationService _service =
        new(new InMemoryProductRepository());

    [Fact]
    public async Task OffsetPagination_Page2_ReturnsProducts6To10()
    {
        var result = await _service.GetPageAsync(2, 5);

        Assert.Equal(
            new[] { 6, 7, 8, 9, 10 },
            result.Items.Select(p => p.Id));

        Assert.Equal(100, result.TotalCount);
        Assert.Equal(20, result.TotalPages);
        Assert.True(result.HasNextPage);
        Assert.True(result.HasPreviousPage);
    }

    [Fact]
    public async Task OffsetPagination_FirstPage_ReturnsProducts1To10()
    {
        var result = await _service.GetPageAsync(1, 10);

        Assert.Equal(
            Enumerable.Range(1, 10),
            result.Items.Select(p => p.Id));

        Assert.False(result.HasPreviousPage);
    }

    [Fact]
    public async Task OffsetPagination_BeyondLastPage_ReturnsEmpty()
    {
        var result = await _service.GetPageAsync(21, 5);

        Assert.Empty(result.Items);
        Assert.False(result.HasNextPage);
    }

    [Fact]
    public async Task CursorPagination_After5_Returns6To10()
    {
        var result = await _service.GetCursorPageAsync(5, 5);

        Assert.Equal(
            new[] { 6, 7, 8, 9, 10 },
            result.Items.Select(p => p.Id));

        Assert.Equal(10, result.NextCursor);
        Assert.True(result.HasNextPage);
    }

    [Fact]
    public async Task CursorPagination_LastPage_HasNoNextCursor()
    {
        var result = await _service.GetCursorPageAsync(95, 5);

        Assert.Equal(
            new[] { 96, 97, 98, 99, 100 },
            result.Items.Select(p => p.Id));

        Assert.Null(result.NextCursor);
        Assert.False(result.HasNextPage);
    }

    [Theory]
    [InlineData(0, 10)]
    [InlineData(1, 0)]
    [InlineData(1, 101)]
    public async Task OffsetPagination_InvalidParameters_Throw(
        int page, int pageSize)
    {
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
            () => _service.GetPageAsync(page, pageSize));
    }

    [Theory]
    [InlineData(-1, 5)]
    [InlineData(0, 0)]
    [InlineData(0, 101)]
    public async Task CursorPagination_InvalidParameters_Throw(
        int afterId, int pageSize)
    {
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
            () => _service.GetCursorPageAsync(afterId, pageSize));
    }
}
