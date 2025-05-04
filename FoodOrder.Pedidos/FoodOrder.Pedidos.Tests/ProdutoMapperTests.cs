using FoodOrder.Pedidos.Application.DTOs.Produto;
using FoodOrder.Pedidos.Application.Mapper;

namespace FoodOrder.Pedidos.Tests;

public class ProdutoMapperTests
{
    [Fact]
    public void Map_DeveMapearProdutoDtoParaProdutoOutputCorretamente()
    {
        // Arrange
        var produtoDto = new ProdutoDto
        {
            Id = 1,
            Nome = "Pizza",
            Descricao = "Pizza de calabresa"
        };

        // Act
        var produtoOutput = ProdutoMapper.Map(produtoDto);

        // Assert
        Assert.NotNull(produtoOutput);
        Assert.Equal(produtoDto.Id, produtoOutput.Id);
        Assert.Equal(produtoDto.Nome, produtoOutput.Nome);
        Assert.Equal(produtoDto.Descricao, produtoOutput.Descricao);
    }
}
