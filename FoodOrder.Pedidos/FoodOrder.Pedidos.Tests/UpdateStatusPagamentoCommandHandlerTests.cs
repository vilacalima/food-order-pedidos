using FoodOrder.Pedidos.Application.DTOs.Pedidos;
using FoodOrder.Pedidos.Application.Feature.Pedidos;
using FoodOrder.Pedidos.Application.UseCase.Pedidos.Interface;
using FoodOrder.Pedidos.Domain.Enums;
using FoodOrder.Pedidos.Domain.Messaging;
using MediatR;
using Moq;

namespace FoodOrder.Pedidos.Tests
{
    public class UpdateStatusPagamentoCommandHandlerTests
    {
        [Fact]
        public async Task Handle_DeveAtualizarStatusPagamentoEEnviarParaProducao_QuandoPagamentoRealizado()
        {
            // Arrange
            var pedidoUseCaseMock = new Mock<IPedidoUseCase>();
            var sqsMock = new Mock<ISqsMessageSender>();

            var pedido = new PedidoOutput
            {
                NumeroPedido = 123,
                PagamentoStatus = PagamentoStatusEnum.AguardandoPagamento,
                PedidoStatus = PedidoStatusEnum.EmPreparacao
            };

            pedidoUseCaseMock
                .Setup(x => x.Consultar(It.IsAny<int>()))
                .ReturnsAsync(pedido);

            var handler = new UpdateStatusPagamentoCommandHandler(pedidoUseCaseMock.Object, sqsMock.Object);

            var command = new UpdateStatusPagamentoCommand(123, PagamentoStatusEnum.PagamentoRealizado);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            Assert.Equal(Unit.Value, result);
            pedidoUseCaseMock.Verify(x => x.Consultar(command.NumeroPedido), Times.Once);
            pedidoUseCaseMock.Verify(x => x.AtualizarStatusPagamento(It.IsAny<PedidoOutput>()), Times.Once);
            sqsMock.Verify(x => x.EnviarMensagemAsync(It.IsAny<PedidoOutput>(), "producao"), Times.Once);
        }

        [Fact]
        public async Task Handle_NaoDeveEnviarParaProducao_SePagamentoNaoFoiRealizado()
        {
            // Arrange
            var pedidoUseCaseMock = new Mock<IPedidoUseCase>();
            var sqsMock = new Mock<ISqsMessageSender>();

            var pedido = new PedidoOutput
            {
                NumeroPedido = 456,
                PagamentoStatus = PagamentoStatusEnum.AguardandoPagamento,
                PedidoStatus = PedidoStatusEnum.EmPreparacao
            };

            pedidoUseCaseMock
                .Setup(x => x.Consultar(It.IsAny<int>()))
                .ReturnsAsync(pedido);

            var handler = new UpdateStatusPagamentoCommandHandler(pedidoUseCaseMock.Object, sqsMock.Object);

            var command = new UpdateStatusPagamentoCommand(456, PagamentoStatusEnum.PagamentoRejeitado);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            Assert.Equal(Unit.Value, result);
            sqsMock.Verify(x => x.EnviarMensagemAsync(It.IsAny<PedidoOutput>(), "producao"), Times.Never);
        }

        [Fact]
        public async Task Handle_DeveLancarExcecao_SePedidoNaoForEncontrado()
        {
            // Arrange
            var pedidoUseCaseMock = new Mock<IPedidoUseCase>();
            var sqsMock = new Mock<ISqsMessageSender>();

            pedidoUseCaseMock
                .Setup(x => x.Consultar(It.IsAny<int>()))
                .ReturnsAsync((PedidoOutput?)null);

            var handler = new UpdateStatusPagamentoCommandHandler(pedidoUseCaseMock.Object, sqsMock.Object);
            var command = new UpdateStatusPagamentoCommand(999, PagamentoStatusEnum.PagamentoRealizado);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => handler.Handle(command, default));
        }
    }
}
