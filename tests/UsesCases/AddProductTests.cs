using System.Net.Http.Json;
using Api.Models;
using Bogus;
using Integration.Tests.Config;
using Xunit.Abstractions;

namespace Integration.Tests.UsesCases;

public sealed class AddProductTests : IntegrationTests
{
    private readonly IntegrationTestsFactory _factory;

    public AddProductTests(IntegrationTestsFactory factory, ITestOutputHelper output)
    {
        _factory = factory;
        _factory.Output = output;
    }

    [Fact]
    public async Task NewPrduct1_Post_StatusCode201AndId()
    {
        // Arrange
        var product = new Faker<ProductRequest>()
            .RuleFor(p => p.Name, s => s.Commerce.ProductName())
            .RuleFor(p => p.Quantity, s => s.Random.Int(1, 100))
            .Generate();


        // Act
        var act = await _factory.CreateClient()
            .PostAsync(
                "/products",
                JsonContent.Create(product));


        // Assert
        act.Should()
           .Be201Created()
           .And.Satisfy<ProductResponse>(model =>
                model.Should().Match<ProductResponse>(m =>
                    m.Id > 100 &&
                    m.Name == product.Name &&
                    m.Quantity == product.Quantity));
    }

    [Fact]
    public async Task NewPrduct2_Post_StatusCode201AndId()
    {
        // Arrange
        var product = new Faker<ProductRequest>()
            .RuleFor(p => p.Name, s => s.Commerce.ProductName())
            .RuleFor(p => p.Quantity, s => s.Random.Int(1, 100))
            .Generate();


        // Act
        var act = await _factory.CreateClient()
            .PostAsync(
                "/products",
                JsonContent.Create(product));


        // Assert
        act.Should()
           .Be201Created()
           .And.Satisfy<ProductResponse>(model =>
                model.Should().Match<ProductResponse>(m =>
                    m.Id > 100 &&
                    m.Name == product.Name &&
                    m.Quantity == product.Quantity));
    }
}
