using Api.Infrastructure;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api;

internal static class ProductsEndpoints
{
    public static void MapProductsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/products");

        group.MapGet("", GetProducts)
             .WithName(nameof(GetProducts));

        group.MapGet("{id}", GetProduct)
             .WithName(nameof(GetProduct));

        group.MapPost("", CreateProduct)
             .WithName(nameof(CreateProduct));

        group.MapPut("{id}", UpdateProduct)
             .WithName(nameof(UpdateProduct));

        group.MapDelete("{id}", DeleteProduct)
             .WithName(nameof(DeleteProduct));
    }

    public static async Task<IResult> GetProducts(DatabaseContext dbContext)
    {
        var products = await dbContext
            .Products
            .Select(product => (ProductResponse)product)
            .ToListAsync();

        return Results.Ok(products);
    }

    public static async Task<IResult> GetProduct(int id, DatabaseContext dbContext)
    {
        if(await dbContext.Products.FindAsync(id) is Product product)
        {
            return Results.Ok((ProductResponse)product);
        }

        return Results.NotFound();
    }

    public static async Task<IResult> CreateProduct(ProductRequest request, DatabaseContext dbContext)
    {
        var product = Product.Create(
           request.Name,
           request.Quantity);

        dbContext.Add(product);
        await dbContext.SaveChangesAsync();

        return Results.CreatedAtRoute(
            nameof(GetProduct),
            new { id = product.Id.ToString() },
            (ProductResponse)product);
    }

    public static async Task<IResult> UpdateProduct(int id, ProductRequest request, DatabaseContext dbContext)
    {
        if(await dbContext.Products.FindAsync(id) is Product product)
        {
            product.Name = request.Name;
            product.Quantity = request.Quantity;

            dbContext.Update(product);
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        }

        return Results.NotFound();
    }

    public static async Task<IResult> DeleteProduct(int id, DatabaseContext dbContext)
    {
        if(await dbContext.Products.FindAsync(id) is Product product)
        {
            dbContext.Remove(product);
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        }

        return Results.NotFound();
    }
}
