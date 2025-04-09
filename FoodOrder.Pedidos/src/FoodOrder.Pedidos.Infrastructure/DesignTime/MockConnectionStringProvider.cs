using FoodOrder.Pedidos.Infrastructure.Configurations;

namespace FoodOrder.Pedidos.Infrastructure.DesignTime
{
    public class MockConnectionStringProvider : IConnectionStringProvider
    {
        private readonly string _connectionString;

        public MockConnectionStringProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }

        public string GetConnectionString(string name)
        {
            throw new NotImplementedException();
        }
    }
}
