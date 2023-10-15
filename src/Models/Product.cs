namespace Api.Models;

public sealed class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public int Quantity { get; set; } = default!;

    private Product() { }

    public static Product Create(string name, int quantity)
        => new()
        {
            Name = name,
            Quantity = quantity
        };
}
