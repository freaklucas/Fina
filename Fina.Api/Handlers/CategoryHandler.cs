using Fina.Api.Data;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Categories;
using Fina.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Fina.Api.Handlers;

/// <summary>
///     DataContext => Ainda trabalha em memória até SaveChanges
///     Enquanto não executo .toList() ou .First... não executa query
/// </summary>
/// <param name="context"></param>
public class CategoryHandler(AppDbContext context) : ICategoryHandler
{
    private readonly AppDbContext _context = context;

    public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
    {
        try
        {
            var category = new Category
            {
                UserId = request.UserId,
                Title = request.Title,
                Description = request.Description
            };

            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            return new Response<Category?>(category, 201, "Categoria criada.");
        }
        catch
        {
            return new Response<Category?>(null, 500, "Categoria não criada.");
        }
    }

    public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
    {
        try
        {
            var category = await _context
                .Categories
                .FirstOrDefaultAsync
                    (c => c.Id == request.Id && c.UserId == request.UserId);

            if (category is null) return new Response<Category?>(null, 404, "Usuário/categoria não encontrado(s).");

            category.Title = request.Title;
            category.Description = request.Description;

            _context.Categories.Update(category);
            await context.SaveChangesAsync();

            return new Response<Category?>(category, 200, "Categoria alterada.");
        }
        catch
        {
            return new Response<Category?>(null, 500, "Categoria não criada.");
        }
    }

    public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
    {
        try
        {
            var category = await _context
                .Categories
                .FirstOrDefaultAsync
                    (c => c.Id == request.Id && c.UserId == request.UserId);

            if (category is null) return new Response<Category?>(null, 404, "Usuário/categoria não encontrado(s).");

            _context.Categories.Remove(category);
            await context.SaveChangesAsync();

            return new Response<Category?>(category, 200, "Categoria Removida.");
        }
        catch
        {
            return new Response<Category?>(null, 500, "Categoria não excluída.");
        }
    }

    public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
    {
        try
        {
            var category = await _context
                .Categories
                .AsNoTracking()
                .FirstOrDefaultAsync
                    (c => c.Id == request.Id && c.UserId == request.UserId);

            return category is null
                ? new Response<Category?>(null, 400, "Categoria não encontrada.")
                : new Response<Category?>(category);
        }
        catch
        {
            return new Response<Category?>(null, 500, "Categoria não excluída.");
        }
    }

    public async Task<PagedResponse<List<Category>?>> GetAllAsync(GetAllCategoryRequest request)
    {
        try
        {
            var query = _context
                .Categories
                .AsNoTracking()
                .Where(x => x.UserId == request.UserId)
                .OrderBy(x => x.Title);

            var categories = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var count = await query.CountAsync();

            return new PagedResponse<List<Category>?>(categories, count, request.PageNumber, request.PageSize);
        }
        catch (Exception e)
        {
            return new PagedResponse<List<Category>?>(null, 500, "Não foi possível consultar as categorias.");
        }
    }
}