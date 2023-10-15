namespace Api.Models;

public sealed record ProductRequest
{
    public string Name { get; init; } = default!;
    public int Quantity { get; init; } = default!;
}
