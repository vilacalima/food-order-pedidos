using FoodOrder.Pedidos.Application.DTOs.Pedidos;
using FoodOrder.Pedidos.Application.Feature.Pedidos;
using FoodOrder.Pedidos.Application.UseCase.Pedidos.Interface;
using FoodOrder.Pedidos.Domain.Enums;
using MediatR;
using Moq;

public class UpdateStatusPedidoCommandHandlerTests
{
    [Fact]
    public async Task Handle_DeveAtualizarStatusPedido_QuandoPedidoExistir()
    {
        // Arrange
        var pedidoUseCaseMock = new Mock<IPedidoUseCase>();

        var pedido = new PedidoOutput
        {
            NumeroPedido = 123,
            PagamentoStatus = PagamentoStatusEnum.PagamentoRealizado,
            PedidoStatus = PedidoStatusEnum.EmPreparacao
        };

        pedidoUseCaseMock
            .Setup(x => x.Consultar(It.IsAny<int>()))
            .ReturnsAsync(pedido);

        var handler = new UpdateStatusPedidoCommandHandler(pedidoUseCaseMock.Object);

        var command = new UpdateStatusPedidoCommand(123, PedidoStatusEnum.Pronto);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        Assert.Equal(Unit.Value, result);
        pedidoUseCaseMock.Verify(x => x.Consultar(command.NumeroPedido), Times.Once);
        pedidoUseCaseMock.Verify(x => x.AtualizarStatusPedido(It.IsAny<PedidoOutput>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveLancarExcecao_SePedidoNaoForEncontrado()
    {
        // Arrange
        var pedidoUseCaseMock = new Mock<IPedidoUseCase>();

        pedidoUseCaseMock
            .Setup(x => x.Consultar(It.IsAny<int>()))
            .ReturnsAsync((PedidoOutput?)null);

        var handler = new UpdateStatusPedidoCommandHandler(pedidoUseCaseMock.Object);
        var command = new UpdateStatusPedidoCommand(999, PedidoStatusEnum.Recebido);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => handler.Handle(command, default));
    }
}
