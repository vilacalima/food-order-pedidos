using FoodOrder.Pedidos.Application.DTOs.Pedidos;
using FoodOrder.Pedidos.Application.DTOs.Produto;
using FoodOrder.Pedidos.Application.Feature.Checkout;
using FoodOrder.Pedidos.Application.UseCase.Pedidos.Interface;
using FoodOrder.Pedidos.Domain.Enums;
using FoodOrder.Pedidos.Domain.Messaging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrder.Pedidos.Tests
{
    public class CheckoutCommandHandlerTests
    {
        [Fact]
        public async Task Handle_DeveCriarPedidoEEnviarParaPagamento_QuandoCheckoutForValido()
        {
            // Arrange
            var pedidoUseCaseMock = new Mock<IPedidoUseCase>();
            var sqsMock = new Mock<ISqsMessageSender>();

            var pedidoEsperado = new PedidoDto(numeroPedido: 123, preco: 50);

            pedidoEsperado.SetMetodoPagamento(MetodoPagamento.Pix);

            pedidoUseCaseMock
                .Setup(x => x.CriarNovoPedido(It.IsAny<List<int>>(), It.IsAny<Guid>()))
                .ReturnsAsync(pedidoEsperado);

            var handler = new CheckoutCommandHandler(pedidoUseCaseMock.Object, sqsMock.Object);

            var command = new CheckoutCommand
            {
                ClienteId = Guid.NewGuid(),
                MetodoPagamento = MetodoPagamento.Pix,
                Produtos = new List<int> { 1, 2, 3 }
            };

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            Assert.Equal(pedidoEsperado.NumeroPedido, result.NumeroPedido);
            Assert.Equal(MetodoPagamento.Pix, result.MetodoPagamento);

            pedidoUseCaseMock.Verify(x => x.CriarNovoPedido(command.Produtos, command.ClienteId), Times.Once);
            sqsMock.Verify(x => x.EnviarMensagemAsync(It.IsAny<PedidoDto>(), "checkout"), Times.Once);
        }
    }
}
