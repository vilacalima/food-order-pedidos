using FoodOrder.Pedidos.Infrastructure.Data;
using FoodOrder.Pedidos.Infrastructure.DesignTime;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FoodOrder.Pedidos.Infrastructure.Factories
{
    public class PedidosDbContextFactory : IDesignTimeDbContextFactory<PedidosDbContext>
    {
        public PedidosDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PedidosDbContext>();

            var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("A variável de ambiente 'DefaultConnection' não foi definida.");
            }

            optionsBuilder.UseNpgsql(connectionString);

            return new PedidosDbContext(optionsBuilder.Options);
        }
    }
}
