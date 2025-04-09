using Microsoft.Extensions.DependencyInjection;

namespace FoodOrder.Pedidos.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceCollection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            

            // services.AddScoped<IRepository, Repository>();
            // services.AddDbContext<YourDbContext>(...);
            return services;
        }
    }
}
