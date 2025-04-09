using Amazon.SQS;
using FoodOrder.Pedidos.Infrastructure.Messaging.Consumers;
using Microsoft.Extensions.DependencyInjection;

namespace FoodOrder.Pedidos.Infrastructure.DependencyInjection
{
    public static class MessageBusServiceCollection
    {
        public static IServiceCollection AddMessageBusServices(this IServiceCollection services)
        {
            //services.AddAWSService<IAmazonSQS>();
            services.AddHostedService<SqsPedidoConsumer>();

            return services;
        }
    }
}