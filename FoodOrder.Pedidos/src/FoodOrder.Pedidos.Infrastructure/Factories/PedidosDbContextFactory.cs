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

            // Substitua pela sua string real de conexão local
            var connectionString = "Host=localhost;Port=5433;Database=foodorder;Username=postgres;Password=postgres";

            optionsBuilder.UseNpgsql(connectionString);

            return new PedidosDbContext(connectionString);
        }
    }
}
