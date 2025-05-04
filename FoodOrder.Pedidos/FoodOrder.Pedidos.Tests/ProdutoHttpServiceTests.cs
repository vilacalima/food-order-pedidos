using FoodOrder.Pedidos.Application.Service;
using Microsoft.Extensions.Configuration;
using RichardSzalay.MockHttp;
using System.Net;

namespace FoodOrder.Pedidos.Tests;
public class ProdutoHttpServiceTests
{
    private readonly IConfiguration _config;
    private readonly string _urlBase = "https://fake-api.com";

    public ProdutoHttpServiceTests()
    {
        var inMemorySettings = new Dictionary<string, string?>
        {
            { "ExternalApis:ProdutosApi", _urlBase }
        };

        _config = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
    }

    [Fact]
    public async Task ObterProdutosAsync_DeveRetornarListaDeProdutos()
    {
        // Arrange
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When($"{_urlBase}/produtos")
                .Respond("application/json", """
                [
                    { "Id": 1, "Nome": "Pizza" },
                    { "Id": 2, "Nome": "Hamburguer" }
                ]
                """);

        var httpClient = new HttpClient(mockHttp);
        var service = new ProdutoHttpService(httpClient, _config);

        // Act
        var result = await service.ObterProdutosAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("Pizza", result[0].Nome);
    }

    [Fact]
    public async Task ObterProdutoPorIdAsync_DeveRetornarProduto_SeExistir()
    {
        // Arrange
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When($"{_urlBase}/Produtos/1")
                .Respond("application/json", """{ "Id": 1, "Nome": "Pizza" }""");

        var httpClient = new HttpClient(mockHttp);
        var service = new ProdutoHttpService(httpClient, _config);

        // Act
        var result = await service.ObterProdutoPorIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result?.Id);
        Assert.Equal("Pizza", result?.Nome);
    }

    [Fact]
    public async Task ObterProdutoPorIdAsync_DeveRetornarNull_SeNaoEncontrado()
    {
        // Arrange
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When($"{_urlBase}/Produtos/999")
                .Respond(HttpStatusCode.NotFound);

        var httpClient = new HttpClient(mockHttp);
        var service = new ProdutoHttpService(httpClient, _config);

        // Act
        var result = await service.ObterProdutoPorIdAsync(999);

        // Assert
        Assert.Null(result);
    }
}
