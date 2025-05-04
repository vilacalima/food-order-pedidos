using FoodOrder.Pedidos.Domain.Entities;
using FoodOrder.Pedidos.Infrastructure.Configurations;
using FoodOrder.Pedidos.Infrastructure.Data;
using FoodOrder.Pedidos.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace FoodOrder.Pedidos.Tests
{
    public class SacolaRepositoryTests
    {
        private readonly Mock<IConnectionStringProvider> _connectionStringProviderMock;
        private readonly DbContextOptions<PedidosDbContext> _options;

        public SacolaRepositoryTests()
        {
            _connectionStringProviderMock = new Mock<IConnectionStringProvider>();
            _connectionStringProviderMock.Setup(provider => provider.GetConnectionString("DefaultConnection"))
                .Returns("Host=localhost;Port=5432;Username=test;Password=test;Database=testdb");

            _options = new DbContextOptionsBuilder<PedidosDbContext>()
                .UseInMemoryDatabase(databaseName: "PedidosDb")
                .Options;
        }

        [Fact]
        public async Task Cadastrar_SacolaValida_DeveAdicionarSacola()
        {
            using var context = new PedidosDbContext(_connectionStringProviderMock.Object, isTesting: true);
            var repository = new SacolaRepository(context);
            var sacola = new Sacola
            {
                DataCriacao = DateTime.UtcNow
            };

            // Act
            var resultado = await repository.Cadastrar(sacola);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(sacola, resultado);
            Assert.Contains(resultado, context.Sacola);
        }

        [Fact]
        public async Task ResgatarUltimaSacola_QuandoNaoExistirSacola_DeveLancarExcecao()
        {
            // Arrange
            using var context = new PedidosDbContext(_connectionStringProviderMock.Object, isTesting: true);
            var repository = new SacolaRepository(context);

            context.Sacola.RemoveRange(context.Sacola);
            await context.SaveChangesAsync();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => repository.ResgatarUltimaSacola());

            Assert.Equal("Nenhuma sacola encontrada.", exception.Message);
        }

        [Fact]
        public async Task ResgatarUltimaSacola_QuandoExistirSacola_DeveRetornarUltimaSacola()
        {
            // Arrange
            using var context = new PedidosDbContext(_connectionStringProviderMock.Object, isTesting: true);
            var repository = new SacolaRepository(context);
            var sacola = new Sacola
            {
                DataCriacao = DateTime.UtcNow
            };
            await repository.Cadastrar(sacola); 

            // Act
            var resultado = await repository.ResgatarUltimaSacola();

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(sacola, resultado);
        }

        [Fact]
        public async Task ConsultarPorSacola_QuandoExistirProdutos_DeveRetornarProdutosCorretos()
        {
            // Arrange
            using var context = new PedidosDbContext(_connectionStringProviderMock.Object, isTesting: true);
            var repository = new SacolaProdutoRepository(context);

            var sacola = new Sacola { DataCriacao = DateTime.UtcNow }; 
            context.Sacola.Add(sacola);
            await context.SaveChangesAsync();

            var produto1 = new SacolaProduto { SacolaId = sacola.Id, ProdutoId = 1 };
            var produto2 = new SacolaProduto { SacolaId = sacola.Id, ProdutoId = 2 };
            context.SacolasProdutos.Add(produto1);
            context.SacolasProdutos.Add(produto2);
            await context.SaveChangesAsync();

            // Act
            var produtos = await repository.ConsultarPorSacola(sacola.Id);

            // Assert
            Assert.NotNull(produtos);
            Assert.Equal(2, produtos.Count);
            Assert.Contains(produtos, p => p.ProdutoId == 1);
            Assert.Contains(produtos, p => p.ProdutoId == 2);
        }

        [Fact]
        public async Task Cadastrar_QuandoSacolaProdutoValido_DeveCadastrarCorretamente()
        {
            // Arrange
            using var context = new PedidosDbContext(_connectionStringProviderMock.Object, isTesting: true);
            var repository = new SacolaProdutoRepository(context);

            var sacola = new Sacola { Id = 1 };
            context.Sacola.Add(sacola);
            await context.SaveChangesAsync();

            var sacolaProduto = new SacolaProduto { SacolaId = sacola.Id, ProdutoId = 5 };

            // Act
            var resultado = await repository.Cadastrar(sacolaProduto);

            // Assert
            Assert.NotNull(resultado);  
            Assert.Equal(5, resultado.ProdutoId); 
        }

        [Fact]
        public async Task Cadastrar_QuandoSacolaProdutoForNull_DeveLancarArgumentNullException()
        {
            // Arrange
            using var context = new PedidosDbContext(_connectionStringProviderMock.Object, isTesting: true);
            var repository = new SacolaProdutoRepository(context);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => repository.Cadastrar(null));
        }

    }
}
