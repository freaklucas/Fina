using Fina.Api.Common.Api;
using Fina.Core;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Categories;
using Fina.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Fina.Api.Endpoints.Categories;

/// <summary>
///     QueryString
///     localhost:8080/v1/categories?pageNumber=1&pageSize=25
/// </summary>
public class GetAllCategoriesEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/", HandleAsync)
            .WithName("Categories: all")
            .WithSummary("Busca todas categorias")
            .WithDescription("Busca todas as categorias")
            .WithOrder(5)
            .Produces<PagedResponse<Category?>>();
    }

    private static async Task<IResult> HandleAsync(
        ICategoryHandler handler,
        [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
        [FromQuery] int pageSize = Configuration.DefaultPageSize
    )
    {
        var request = new GetAllCategoryRequest
        {
            PageSize = Configuration.DefaultPageSize,
            PageNumber = Configuration.DefaultPageNumber,
            UserId = ApiConfiguration.UserId
        };


        var result = await handler.GetAllAsync(request);

        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}