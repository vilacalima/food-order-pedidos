
using Amazon.SQS;
using Amazon.SQS.Model;
using FoodOrder.Pedidos.Domain.Messaging;
using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Retry;
using System.Text.Json;

namespace FoodOrder.Pedidos.Infrastructure.Messaging.Producers
{
    public class SqsMessageSender : ISqsMessageSender
    {
        private readonly IAmazonSQS _sqsClient;
        private readonly IConfiguration _configuration;
        private readonly AsyncRetryPolicy _retryPolicy;

        private string QueueUrl = string.Empty;

        public SqsMessageSender(IAmazonSQS sqsClient, IConfiguration configuration)
        {
            _sqsClient = sqsClient;
            _configuration = configuration;

            var url = _configuration["MercadoPago:EndPoints:CriarPagamento"];

            _retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                    onRetry: (exception, time) =>
                    {
                        Console.WriteLine($"Erro ao enviar para o SQS. Tentando novamente em {time.TotalSeconds}s...");
                    });
        }

        public async Task EnviarMensagemAsync<T>(T mensagem, string queueUrl)
        {
            var jsonBody = JsonSerializer.Serialize(mensagem);

            var request = new SendMessageRequest
            {
                QueueUrl = queueUrl,
                MessageBody = jsonBody
            };

            await _retryPolicy.ExecuteAsync(async () =>
            {
                var response = await _sqsClient.SendMessageAsync(request);
                Console.WriteLine($"Mensagem enviada para SQS com ID: {response.MessageId}");
            });
        }
    }
}
