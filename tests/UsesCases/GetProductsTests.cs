using Api.Models;
using Integration.Tests.Config;
using Xunit.Abstractions;

namespace Integration.Tests.UsesCases;

public sealed class GetProductsTests : IntegrationTests
{
    private readonly IntegrationTestsFactory _factory;

    public GetProductsTests(IntegrationTestsFactory factory, ITestOutputHelper output)
    {
        _factory = factory;
        _factory.Output = output;
    }


    [Fact]
    public async Task All_Get_StatusCode200And100Products()
    {
        // Arrange && Act
        var act = await _factory.CreateClient()
            .GetAsync("/products");


        // Assert
        act.Should()
           .Be200Ok()
           .And.Satisfy<IEnumerable<ProductResponse>>(model =>
                model.Should().HaveCount(100));
    }
}
