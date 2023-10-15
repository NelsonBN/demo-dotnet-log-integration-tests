namespace Api.Models;

public sealed record ProductResponse
{
    public int Id { get; init; } = default!;
    public string Name { get; init; } = default!;
    public int Quantity { get; init; } = default!;


    public static implicit operator ProductResponse(Product product)
        => new()
        {
            Id = product.Id,
            Name = product.Name,
            Quantity = product.Quantity
        };
}
