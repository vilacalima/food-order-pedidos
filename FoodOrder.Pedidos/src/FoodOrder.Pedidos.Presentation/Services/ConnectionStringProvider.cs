using FoodOrder.Pedidos.Infrastructure.Configurations;
using Microsoft.Extensions.Configuration;

namespace FoodOrder.Pedidos.Presentation.Services
{
    public class ConnectionStringProvider : IConnectionStringProvider
    {
        private readonly IConfiguration _configuration;

        public ConnectionStringProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnectionString(string name) => _configuration.GetConnectionString(name);
    }
}