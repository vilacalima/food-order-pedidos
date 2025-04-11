using Amazon.SQS;
using FoodOrder.Pedidos.Infrastructure.Messaging.Consumers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FoodOrder.Pedidos.Infrastructure.DependencyInjection
{
    public static class MessageBusServiceCollection
    {
        public static IServiceCollection AddMessageBusServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDefaultAWSOptions(new Amazon.Extensions.NETCore.Setup.AWSOptions
            {
                Region = Amazon.RegionEndpoint.SAEast1
            });

            services.AddAWSService<IAmazonSQS>();
            services.AddHostedService<SqsPedidoConsumer>();

            return services;
        }
    }
}