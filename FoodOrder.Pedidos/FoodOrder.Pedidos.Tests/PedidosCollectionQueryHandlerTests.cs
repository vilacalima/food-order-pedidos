using FoodOrder.Pedidos.Application.DTOs.Pedidos;
using FoodOrder.Pedidos.Application.DTOs.Produto;
using FoodOrder.Pedidos.Application.Feature.Pedidos;
using FoodOrder.Pedidos.Application.UseCase.Pedidos.Interface;
using FoodOrder.Pedidos.Domain.Enums;
using Moq;

namespace FoodOrder.Pedidos.Tests;

public class PedidosCollectionQueryHandlerTests
{
    [Fact]
    public async Task Handle_DeveRetornarPedidosOutputComDadosCorretos()
    {
        // Arrange
        var mockPedidoUseCase = new Mock<IPedidoUseCase>();

        var produto = new ProdutoOutput(1, "Hambúrguer", "Delicioso hambúrguer artesanal");

        var pedidoPronto = new PedidoOutput
        {
            Id = 1,
            NumeroPedido = 1001,
            TempoEspera = TimeSpan.FromMinutes(5),
            DataCriacao = DateTime.UtcNow.AddMinutes(-10),
            ClienteId = Guid.NewGuid(),
            PedidoStatus = PedidoStatusEnum.Pronto,
            PagamentoStatus = PagamentoStatusEnum.PagamentoRealizado,
            SacolaId = 55,
            Produtos = new List<ProdutoOutput> { produto }
        };

        var outputEsperado = new PedidosOutput
        {
            Pronto = new List<PedidoOutput> { pedidoPronto },
            EmPreparacao = [],
            Recebido = [],
            Finalizado = []
        };

        mockPedidoUseCase
            .Setup(x => x.ListarPedidos())
            .ReturnsAsync(outputEsperado);

        var handler = new PedidosCollectionQueryHandler(mockPedidoUseCase.Object);
        var query = new PedidosCollectionQuery();

        // Act
        var resultado = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(resultado);
        Assert.Single(resultado.Pronto);

        var pedido = resultado.Pronto[0];
        Assert.Equal(1, pedido.Id);
        Assert.Equal(1001, pedido.NumeroPedido);
        Assert.Equal(PedidoStatusEnum.Pronto, pedido.PedidoStatus);
        Assert.Equal(PagamentoStatusEnum.PagamentoRealizado, pedido.PagamentoStatus);
        Assert.Single(pedido.Produtos);
        Assert.Equal("Hambúrguer", pedido.Produtos[0].Nome);
    }
}
