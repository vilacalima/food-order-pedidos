using FluentAssertions;
using FoodOrder.Pedidos.Application.DTOs.Pedidos;
using FoodOrder.Pedidos.Application.DTOs.Produto;
using FoodOrder.Pedidos.Application.Interfaces;
using FoodOrder.Pedidos.Application.UseCase.Pedidos;
using FoodOrder.Pedidos.Domain.Entities;
using FoodOrder.Pedidos.Domain.Enums;
using FoodOrder.Pedidos.Domain.Repository;
using Moq;

namespace FoodOrder.Pedidos.Tests
{
    public class PedidoUseCaseTests
    {
        [Fact]
        public async Task CriarNovoPedido_DeveRetornarPedidoDtoComPrecoCorreto()
        {
            // Arrange
            var mockPedidoRepo = new Mock<IPedidoRepository>();
            var mockSacolaRepo = new Mock<ISacolaRepository>();
            var mockSacolaProdutoRepo = new Mock<ISacolaProdutoRepository>();
            var mockProdutoService = new Mock<IProdutoHttpService>();

            var clienteId = Guid.NewGuid();
            var produtosIds = new List<int> { 1, 2 };

            // Simula criação da sacola
            var sacola = new Sacola { Id = 123 };
            mockSacolaRepo.Setup(r => r.Cadastrar(It.IsAny<Sacola>()))
                          .ReturnsAsync(sacola);

            // Simula o cadastro do pedido
            mockPedidoRepo.Setup(r => r.Cadastrar(It.IsAny<Pedido>()))
                          .ReturnsAsync(999); // numero do pedido

            // Simula produtos retornados da API externa
            mockProdutoService.Setup(s => s.ObterProdutoPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => new ProdutoDto
                {
                    Id = id,
                    Nome = $"Produto {id}",
                    Preco = 10,
                    TempoPreparo = 15,
                    Tipo = "Lanche"
                });

            var useCase = new PedidoUseCase(
                mockPedidoRepo.Object,
                mockSacolaProdutoRepo.Object,
                mockProdutoService.Object,
                mockSacolaRepo.Object
            );

            // Act
            var resultado = await useCase.CriarNovoPedido(produtosIds, clienteId);

            // Assert
            resultado.Should().NotBeNull();
            resultado.NumeroPedido.Should().Be(999);
            resultado.Preco.Should().Be(20); // 2 produtos x 10
            mockSacolaProdutoRepo.Verify(r => r.Cadastrar(It.IsAny<SacolaProduto>()), Times.Exactly(2));
            mockPedidoRepo.Verify(r => r.Cadastrar(It.IsAny<Pedido>()), Times.Once);
        }


        [Fact]
        public async Task AtualizarStatusPedido_DeveChamarRepositorioComIdECorretamente()
        {
            // Arrange
            var mockPedidoRepo = new Mock<IPedidoRepository>();
            var useCase = new PedidoUseCase(
                mockPedidoRepo.Object,
                Mock.Of<ISacolaProdutoRepository>(),
                Mock.Of<IProdutoHttpService>(),
                Mock.Of<ISacolaRepository>()
            );

            var pedidoAtualizado = new PedidoOutput
            {
                Id = 123,
                PedidoStatus = PedidoStatusEnum.EmPreparacao
            };

            // Act
            await useCase.AtualizarStatusPedido(pedidoAtualizado);

            // Assert
            mockPedidoRepo.Verify(
                r => r.AtualizarStatusPedido(pedidoAtualizado.Id, pedidoAtualizado.PedidoStatus),
                Times.Once
            );
        }

        [Fact]
        public async Task AtualizarStatusPedido_NaoDeveChamarRepositorio_SePedidoInvalido()
        {
            // Arrange
            var mockPedidoRepo = new Mock<IPedidoRepository>();
            var useCase = new PedidoUseCase(
                mockPedidoRepo.Object,
                Mock.Of<ISacolaProdutoRepository>(),
                Mock.Of<IProdutoHttpService>(),
                Mock.Of<ISacolaRepository>()
            );

            var pedidoInvalido = new PedidoOutput(); // Id padrão 0

            // Act
            await useCase.AtualizarStatusPedido(pedidoInvalido);

            // Assert
            mockPedidoRepo.Verify(
                r => r.AtualizarStatusPedido(It.IsAny<int>(), It.IsAny<PedidoStatusEnum>()),
                Times.Never
            );
        }

        [Fact]
        public async Task ListarPedidos_DeveRetornarPedidosSeparadosPorStatus()
        {
            // Arrange
            var mockPedidoRepo = new Mock<IPedidoRepository>();
            var mockSacolaProdutoRepo = new Mock<ISacolaProdutoRepository>();
            var mockProdutoService = new Mock<IProdutoHttpService>();
            var mockSacolaRepo = new Mock<ISacolaRepository>();

            var pedidos = new List<Pedido>
            {
                new Pedido { Id = 1, PedidoStatus = PedidoStatusEnum.Recebido, DataCriacao = DateTime.Now.AddMinutes(-10), SacolaId = 1 },
                new Pedido { Id = 2, PedidoStatus = PedidoStatusEnum.Pronto, DataCriacao = DateTime.Now.AddMinutes(-5), SacolaId = 2 },
                new Pedido { Id = 3, PedidoStatus = PedidoStatusEnum.EmPreparacao, DataCriacao = DateTime.Now.AddMinutes(-8), SacolaId = 3 }
            };

            mockPedidoRepo.Setup(r => r.ListarPedidos()).ReturnsAsync(pedidos);
            mockSacolaProdutoRepo.Setup(r => r.ConsultarPorSacola(It.IsAny<int>())).ReturnsAsync(new List<SacolaProduto>());
            mockProdutoService.Setup(p => p.ObterProdutoPorIdAsync(It.IsAny<int>())).ReturnsAsync((ProdutoDto)null);

            var useCase = new PedidoUseCase(
                mockPedidoRepo.Object,
                mockSacolaProdutoRepo.Object,
                mockProdutoService.Object,
                mockSacolaRepo.Object
            );

            // Act
            var resultado = await useCase.ListarPedidos();

            // Assert
            resultado.Recebido.Should().HaveCount(1);
            resultado.Pronto.Should().HaveCount(1);
            resultado.EmPreparacao.Should().HaveCount(1);
            resultado.Finalizado.Should().BeEmpty();
        }

        [Fact]
        public async Task Consultar_DeveRetornarPedidoOutput_SeEncontrado()
        {
            // Arrange
            var pedidoNumero = 1;
            var pedidoMock = new Pedido
            {
                Id = 10,
                NumeroPedido = pedidoNumero,
                PedidoStatus = PedidoStatusEnum.Recebido,
                SacolaId = 5
            };

            var mockPedidoRepo = new Mock<IPedidoRepository>();
            var mockSacolaProdutoRepo = new Mock<ISacolaProdutoRepository>();
            var mockProdutoService = new Mock<IProdutoHttpService>();
            var mockSacolaRepo = new Mock<ISacolaRepository>();

            mockPedidoRepo.Setup(r => r.ConsultarPedidoPorNumero(pedidoNumero)).ReturnsAsync(pedidoMock);
            mockSacolaProdutoRepo.Setup(r => r.ConsultarPorSacola(pedidoMock.SacolaId)).ReturnsAsync(new List<SacolaProduto>());
            mockProdutoService.Setup(p => p.ObterProdutoPorIdAsync(It.IsAny<int>())).ReturnsAsync((ProdutoDto)null);

            var useCase = new PedidoUseCase(
                mockPedidoRepo.Object,
                mockSacolaProdutoRepo.Object,
                mockProdutoService.Object,
                mockSacolaRepo.Object
            );

            // Act
            var resultado = await useCase.Consultar(pedidoNumero);

            // Assert
            resultado.Should().NotBeNull();
            resultado.NumeroPedido.Should().Be(pedidoNumero);
        }

        [Fact]
        public async Task Consultar_DeveLancarExcecao_SePedidoNaoEncontrado()
        {
            // Arrange
            var numeroPedido = 999;
            var mockPedidoRepo = new Mock<IPedidoRepository>();

            mockPedidoRepo.Setup(r => r.ConsultarPedidoPorNumero(numeroPedido)).ReturnsAsync((Pedido)null);

            var useCase = new PedidoUseCase(
                mockPedidoRepo.Object,
                Mock.Of<ISacolaProdutoRepository>(),
                Mock.Of<IProdutoHttpService>(),
                Mock.Of<ISacolaRepository>()
            );

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => useCase.Consultar(numeroPedido));
        }

        [Fact]
        public async Task AtualizarStatusPagamento_DeveChamarRepositorioComIdECorretamente()
        {
            // Arrange
            var mockPedidoRepo = new Mock<IPedidoRepository>();

            var useCase = new PedidoUseCase(
                mockPedidoRepo.Object,
                Mock.Of<ISacolaProdutoRepository>(),
                Mock.Of<IProdutoHttpService>(),
                Mock.Of<ISacolaRepository>()
            );

            var pedidoAtualizado = new PedidoOutput
            {
                Id = 101,
                PagamentoStatus = PagamentoStatusEnum.PagamentoRealizado
            };

            // Act
            await useCase.AtualizarStatusPagamento(pedidoAtualizado);

            // Assert
            mockPedidoRepo.Verify(
                r => r.AtualizarStatusPagamento(pedidoAtualizado.Id, pedidoAtualizado.PagamentoStatus),
                Times.Once
            );
        }

        [Fact]
        public async Task AtualizarStatusPagamento_NaoDeveChamarRepositorio_SePedidoInvalido()
        {
            // Arrange
            var mockPedidoRepo = new Mock<IPedidoRepository>();

            var useCase = new PedidoUseCase(
                mockPedidoRepo.Object,
                Mock.Of<ISacolaProdutoRepository>(),
                Mock.Of<IProdutoHttpService>(),
                Mock.Of<ISacolaRepository>()
            );

            var pedidoInvalido = new PedidoOutput(); // Id 0

            // Act
            await useCase.AtualizarStatusPagamento(pedidoInvalido);

            // Assert
            mockPedidoRepo.Verify(
                r => r.AtualizarStatusPagamento(It.IsAny<int>(), It.IsAny<PagamentoStatusEnum>()),
                Times.Never
            );
        }

        [Fact]
        public async Task CriarNovoPedido_DeveCriarPedidoERetornarPedidoDto()
        {
            // Arrange
            var produtoIds = new List<int> { 1, 2 };
            var clienteId = Guid.NewGuid();

            var sacolaRepoMock = new Mock<ISacolaRepository>();
            var sacolaProdutoRepoMock = new Mock<ISacolaProdutoRepository>();
            var produtoServiceMock = new Mock<IProdutoHttpService>();
            var pedidoRepoMock = new Mock<IPedidoRepository>();

            // Simula sacola criada
            sacolaRepoMock.Setup(r => r.Cadastrar(It.IsAny<Sacola>()))
                          .ReturnsAsync(new Sacola { Id = 10 });

            // Simula cadastro de produto na sacola
            sacolaProdutoRepoMock.Setup(r => r.Cadastrar(It.IsAny<SacolaProduto>()))
                                 .ReturnsAsync(new SacolaProduto(10, 1));  // Retorna um SacolaProduto simulado

            // Simula produtos retornados da API
            produtoServiceMock.Setup(s => s.ObterProdutoPorIdAsync(1))
                              .ReturnsAsync(new ProdutoDto { Id = 1, Preco = 10.0m, TempoPreparo = 5, Tipo = "Lanche" });

            produtoServiceMock.Setup(s => s.ObterProdutoPorIdAsync(2))
                              .ReturnsAsync(new ProdutoDto { Id = 2, Preco = 15.0m, TempoPreparo = 7, Tipo = "Lanche" });

            // Simula pedido cadastrado
            pedidoRepoMock.Setup(r => r.Cadastrar(It.IsAny<Pedido>()))
                          .ReturnsAsync(1234); // número do pedido

            var useCase = new PedidoUseCase(
                pedidoRepoMock.Object,
                sacolaProdutoRepoMock.Object,
                produtoServiceMock.Object,
                sacolaRepoMock.Object
            );

            // Act
            var resultado = await useCase.CriarNovoPedido(produtoIds, clienteId);

            // Assert
            resultado.Should().NotBeNull();
            resultado.NumeroPedido.Should().Be(1234);
            resultado.Preco.Should().Be(25.0m); // 10 + 15
        }
    }
}
