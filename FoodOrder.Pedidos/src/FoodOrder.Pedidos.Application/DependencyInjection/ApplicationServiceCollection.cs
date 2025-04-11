using FoodOrder.Pedidos.Application.Feature.Pedidos;
using FoodOrder.Pedidos.Application.Interfaces;
using FoodOrder.Pedidos.Application.Service;
using FoodOrder.Pedidos.Application.UseCase.Pedidos.Interface;
using FoodOrder.Pedidos.Application.UseCase.Pedidos;
using FoodOrder.Pedidos.Domain.Repository;
using FoodOrder.Pedidos.Infrastructure.Data;
using FoodOrder.Pedidos.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FoodOrder.Pedidos.Application.DependencyInjection
{
    public static class ApplicationServiceCollection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            MediatR(services);

            services.AddDbContext<PedidosDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IPedidoUseCase, PedidoUseCase>();

            services.AddScoped<IPedidoRepository, PedidoRepository>();
            services.AddScoped<IPedidoStatusRepository, PedidoStatusRepository>();
            services.AddScoped<ISacolaProdutoRepository, SacolaProdutoRepository>();
            services.AddScoped<ISacolaRepository, SacolaRepository>();

            var produtosApiUrl = configuration["ExternalApis:ProdutosApi"];
            services.AddHttpClient<IProdutoHttpService, ProdutoHttpService>(client =>
            {
                client.BaseAddress = new Uri(produtosApiUrl!);
            });


            return services;
        }

        private static void MediatR(IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    typeof(UpdateStatusPedidoCommandHandler).Assembly,
                    typeof(PedidosCollectionQueryHandler).Assembly
                );
            });
        }
    }
}
