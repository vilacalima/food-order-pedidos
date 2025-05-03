using FoodOrder.Pedidos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FoodOrder.Pedidos.Presentation.Services;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using PedidosDbContext dbContext =
            scope.ServiceProvider.GetRequiredService<PedidosDbContext>();

        dbContext.Database.Migrate();
    }
}