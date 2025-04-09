using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Hosting;

namespace FoodOrder.Pedidos.Infrastructure.Messaging.Consumers
{
    internal class SqsPedidoConsumer(IAmazonSQS sqsClient) : BackgroundService
    {
        private readonly IAmazonSQS _sqsClient = sqsClient;
        private readonly string _queueUrl = "https://sqs.sa-east-1.amazonaws.com/123456789012/minha-fila"; // coloque sua URL real

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var request = new ReceiveMessageRequest
                {
                    QueueUrl = _queueUrl,
                    MaxNumberOfMessages = 5,
                    WaitTimeSeconds = 10
                };

                var response = await _sqsClient.ReceiveMessageAsync(request, stoppingToken);

                foreach (var message in response.Messages)
                {
                    try
                    {
                        Console.WriteLine($"Mensagem recebida: {message.Body}");

                        // TODO: Processar a mensagem aqui...

                        // Apagar a mensagem da fila após o processamento
                        await _sqsClient.DeleteMessageAsync(_queueUrl, message.ReceiptHandle, stoppingToken);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }
    }
}