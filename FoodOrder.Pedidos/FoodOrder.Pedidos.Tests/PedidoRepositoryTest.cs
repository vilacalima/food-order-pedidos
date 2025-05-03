using FoodOrder.Pedidos.Domain.Entities;
using FoodOrder.Pedidos.Domain.Enums;
using FoodOrder.Pedidos.Infrastructure.Configurations;
using FoodOrder.Pedidos.Infrastructure.Data;
using FoodOrder.Pedidos.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrder.Pedidos.Tests
{
    public class PedidoRepositoryTest
    {
        private readonly Mock<IConnectionStringProvider> _connectionStringProviderMock;
        private readonly DbContextOptions<PedidosDbContext> _options;

        public PedidoRepositoryTest()
        {
            _connectionStringProviderMock = new Mock<IConnectionStringProvider>();
            _connectionStringProviderMock.Setup(provider => provider.GetConnectionString("DefaultConnection"))
                .Returns("Host=localhost;Port=5432;Username=test;Password=test;Database=testdb");

            _options = new DbContextOptionsBuilder<PedidosDbContext>()
                .UseInMemoryDatabase(databaseName: "PedidosDb")
                .Options;
        }

        [Fact]
        public async Task Cadastrar_QuandoPedidoValido_DeveAdicionarPedido()
        {
            // Arrange
            using var context = new PedidosDbContext(_connectionStringProviderMock.Object, isTesting: true);
            var repository = new PedidoRepository(context);

            var pedido = new Pedido
            {
                NumeroPedido = 123,
                TempoEspera = TimeSpan.FromMinutes(30),
                ClienteId = Guid.NewGuid(),
                PagamentoStatus = PagamentoStatusEnum.AguardandoPagamento,
                PedidoStatus = PedidoStatusEnum.Recebido,
                SacolaId = 1  
            };

            // Act
            var numeroPedido = await repository.Cadastrar(pedido);

            // Assert
            Assert.True(numeroPedido > 0);
            var pedidoCadastrado = await repository.ConsultarPedidoPorNumero(pedido.NumeroPedido);
            Assert.NotNull(pedidoCadastrado);  
            Assert.Equal(pedido.PedidoStatus, pedidoCadastrado.PedidoStatus); 
        }

        [Fact]
        public async Task ConsultarPedidoPorNumero_QuandoPedidoExistir_DeveRetornarPedido()
        {
            // Arrange
            using var context = new PedidosDbContext(_connectionStringProviderMock.Object, isTesting: true);
            var repository = new PedidoRepository(context);

            var pedido = new Pedido
            {
                TempoEspera = TimeSpan.FromMinutes(30),
                ClienteId = Guid.NewGuid(),
                PagamentoStatus = PagamentoStatusEnum.AguardandoPagamento,
                PedidoStatus = PedidoStatusEnum.Recebido,
                SacolaId = 1
            };
            context.Pedidos.Add(pedido);
            await context.SaveChangesAsync();

            // Act
            var pedidoConsultado = await repository.ConsultarPedidoPorNumero(pedido.NumeroPedido);

            // Assert
            Assert.NotNull(pedidoConsultado); 
            Assert.Equal(pedido.NumeroPedido, pedidoConsultado.NumeroPedido);
        }


        [Fact]
        public async Task ListarPedidos_QuandoExistiremPedidos_DeveRetornarPedidos()
        {
            // Arrange
            using var context = new PedidosDbContext(_connectionStringProviderMock.Object, isTesting: true);
            var repository = new PedidoRepository(context);

            var pedido = new Pedido
            {
                ClienteId = Guid.NewGuid(),
                PedidoStatus = PedidoStatusEnum.Recebido,
                PagamentoStatus = PagamentoStatusEnum.AguardandoPagamento
            };
            context.Pedidos.Add(pedido);
            await context.SaveChangesAsync();

            // Act
            var pedidos = await repository.ListarPedidos();

            // Assert
            Assert.NotEmpty(pedidos);  
        }

        [Fact]
        public async Task ListarPedidos_QuandoNaoExistiremPedidos_DeveRetornarListaVazia()
        {
            // Arrange
            using var context = new PedidosDbContext(_connectionStringProviderMock.Object, isTesting: true);
            var repository = new PedidoRepository(context);

            context.Pedidos.RemoveRange(context.Pedidos);
            await context.SaveChangesAsync();
            // Act
            var pedidos = await repository.ListarPedidos();

            // Assert
            Assert.Empty(pedidos);
        }

        [Fact]
        public async Task Atualizar_QuandoPedidoExistir_DeveAtualizarPedido()
        {
            // Arrange
            using var context = new PedidosDbContext(_connectionStringProviderMock.Object, isTesting: true);
            var repository = new PedidoRepository(context);

            var pedido = new Pedido
            {
                NumeroPedido = 456,
                ClienteId = Guid.NewGuid(),
                PedidoStatus = PedidoStatusEnum.EmPreparacao,
                PagamentoStatus = PagamentoStatusEnum.PagamentoRealizado,
                SacolaId = 1,
                TempoEspera = TimeSpan.FromMinutes(30)
            };
            context.Pedidos.Add(pedido);
            await context.SaveChangesAsync();

            // Act
            pedido.PedidoStatus = PedidoStatusEnum.Pronto;
            await repository.Atualizar(pedido);

            // Assert
            var pedidoAtualizado = await repository.ConsultarPedidoPorNumero(pedido.NumeroPedido);
            Assert.NotNull(pedidoAtualizado);
            Assert.Equal(PedidoStatusEnum.Pronto, pedidoAtualizado!.PedidoStatus);
        }

        [Fact]
        public async Task AtualizarStatusPagamento_QuandoPedidoExistir_DeveAtualizarStatusPagamento()
        {
            // Arrange
            using var context = new PedidosDbContext(_connectionStringProviderMock.Object, isTesting: true);
            var repository = new PedidoRepository(context);

            var pedido = new Pedido
            {
                TempoEspera = TimeSpan.FromMinutes(30),
                ClienteId = Guid.NewGuid(),
                PagamentoStatus = PagamentoStatusEnum.AguardandoPagamento,
                PedidoStatus = PedidoStatusEnum.Recebido,
                SacolaId = 1
            };
            context.Pedidos.Add(pedido);
            await context.SaveChangesAsync();

            // Act
            await repository.AtualizarStatusPagamento(pedido.Id, PagamentoStatusEnum.PagamentoRealizado);

            // Assert
            var pedidoAtualizado = await context.Pedidos.FindAsync(pedido.Id);
            Assert.NotNull(pedidoAtualizado);
            Assert.Equal(PagamentoStatusEnum.PagamentoRealizado, pedidoAtualizado!.PagamentoStatus);
        }
    }
}
