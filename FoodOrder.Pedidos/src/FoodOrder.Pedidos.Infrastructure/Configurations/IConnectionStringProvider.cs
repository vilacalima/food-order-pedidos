namespace FoodOrder.Pedidos.Infrastructure.Configurations;

public interface IConnectionStringProvider
{
    string GetConnectionString(string name);
}
