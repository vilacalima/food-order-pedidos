using FoodOrder.Pedidos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FoodOrder.Pedidos.Infrastructure.Factories
{
    public class PedidosDbContextFactory : IDesignTimeDbContextFactory<PedidosDbContext>
    {
        public PedidosDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PedidosDbContext>();

            // Pegando a connection string da variável de ambiente
            var connectionString = Environment.GetEnvironmentVariable("DEFAULT_CONNECTION");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("A variável de ambiente 'DEFAULT_CONNECTION' não foi definida.");
            }

            optionsBuilder.UseNpgsql(connectionString);

            // Cria um mock de IConnectionStringProvider só para fins de migração
            var connectionStringProvider = new MockConnectionStringProvider(connectionString);

            return new PedidosDbContext(connectionStringProvider);
        }
    }
}
