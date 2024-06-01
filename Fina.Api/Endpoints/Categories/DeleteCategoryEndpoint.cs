using System.Security.Claims;
using Azure;
using Fina.Api.Common.Api;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Categories;

namespace Fina.Api.Endpoints.Categories;

public class DeleteCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/{id}", HandleAsync)
            .WithName("Categories: delete")
            .WithSummary("Remove uma categoria")
            .WithDescription("Remove uma categoria")
            .WithOrder(3)
            .Produces<Response<Category?>>();
    }

    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        ICategoryHandler handler,
        long id)
    {
        var request = new DeleteCategoryRequest
        {
            UserId = ApiConfiguration.UserId,
            Id = id
        };

        var result = await handler.DeleteAsync(request);

        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}