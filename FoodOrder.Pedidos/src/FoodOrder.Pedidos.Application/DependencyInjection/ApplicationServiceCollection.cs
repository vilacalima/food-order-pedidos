using FoodOrder.Pedidos.Application.Interfaces;
using FoodOrder.Pedidos.Application.Service;
using Microsoft.Extensions.DependencyInjection;

namespace FoodOrder.Pedidos.Application.DependencyInjection
{
    public static class ApplicationServiceCollection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //services.AddHttpClient<IProdutoHttpService, ProdutoHttpService>(client =>
            //{
            //    client.BaseAddress = new Uri("https://sua-api-externa.com/api/"); //trocar a rota
            //});
            // services.AddScoped<IRepository, Repository>();
            // services.AddDbContext<YourDbContext>(...);
            return services;
        }
    }
}
