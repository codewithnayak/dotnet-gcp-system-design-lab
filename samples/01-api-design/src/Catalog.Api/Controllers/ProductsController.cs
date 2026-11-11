
using Catalog.Application.Models;
using Catalog.Application.Services;
using Catalog.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers;

[ApiController]
[Route("api/v1/products")]
[Produces("application/json")]
public sealed class ProductsController(
    ProductPaginationService paginationService)
    : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(
        typeof(PagedResult<Product>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PagedResult<Product>>> GetProducts(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        if (page < 1 || pageSize is < 1 or > 100)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid pagination parameters",
                Detail = "Page must be >= 1 and pageSize must be between 1 and 100.",
                Status = StatusCodes.Status400BadRequest
            });
        }

        var result = await paginationService.GetPageAsync(
            page,
            pageSize,
            cancellationToken);

        return Ok(result);
    }


    [HttpGet("cursor")]
    [ProducesResponseType(
        typeof(CursorPagedResult<Product>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CursorPagedResult<Product>>>
        GetByCursor(
            [FromQuery] int? afterId = null,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
    {
        if (afterId < 0 || pageSize is < 1 or > 100)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid cursor pagination parameters",
                Detail = "afterId must be >= 0 and pageSize must be between 1 and 100.",
                Status = StatusCodes.Status400BadRequest
            });
        }

        var result = await paginationService.GetCursorPageAsync(
            afterId,
            pageSize,
            cancellationToken);

        return Ok(result);
    }

}
