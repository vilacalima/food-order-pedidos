
using Amazon.SQS;
using Amazon.SQS.Model;
using FoodOrder.Pedidos.Domain.Messaging;
using Polly;
using Polly.Retry;
using System.Text.Json;

namespace FoodOrder.Pedidos.Infrastructure.Messaging.Producers
{
    public class SqsMessageSender : ISqsMessageSender
    {
        private readonly IAmazonSQS _sqsClient;
        private readonly AsyncRetryPolicy _retryPolicy;

        public SqsMessageSender(IAmazonSQS sqsClient)
        {
            _sqsClient = sqsClient;

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
