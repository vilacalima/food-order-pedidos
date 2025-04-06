using FoodOrder.Pedidos.Infrastructure.Configurations;

namespace FoodOrder.Pedidos.Presentation.Services
{
    public class ConnectionStringProvider : IConnectionStringProvider
    {
        private readonly IConfiguration _configuration;

        public ConnectionStringProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnectionString(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name), "O nome da connection string não pode ser nulo ou vazio.");

            var connectionString = _configuration.GetConnectionString(name);

            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException($"Connection string '{name}' não encontrada.");

            return connectionString!;
        }

    }
}