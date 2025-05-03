using Amazon.SQS;
using Amazon.SQS.Model;
using FoodOrder.Pedidos.Infrastructure.Messaging.Producers;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Text.Json;

namespace FoodOrder.Pedidos.Tests;

public class SqsMessageSenderTests
{
    [Fact]
    public async Task EnviarMensagemAsync_DeveEnviarMensagemComSucesso()
    {
        // Arrange  
        var mockSqs = new Mock<IAmazonSQS>();
        var mockConfig = new Mock<IConfiguration>();
        var fakeQueueUrl = "https://sqs.us-east-1.amazonaws.com/123456789012/checkout";

        // Simula retorno do Amazon SQS  
        mockSqs.Setup(sqs => sqs.SendMessageAsync(It.IsAny<SendMessageRequest>(), default))
               .ReturnsAsync(new SendMessageResponse { MessageId = "12345" });

        var sender = new SqsMessageSender(mockSqs.Object, mockConfig.Object);
        var mensagem = new { Nome = "Teste", Quantidade = 3 };

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var serializedMessage = System.Text.Json.JsonSerializer.Serialize(mensagem, options);

        // Act  
        await sender.EnviarMensagemAsync(mensagem, fakeQueueUrl);
    }

    [Fact]
    public async Task EnviarMensagemAsync_DeveTentarNovamenteSeFalhar()
    {
        // Arrange
        var mockSqs = new Mock<IAmazonSQS>();
        var mockConfig = new Mock<IConfiguration>();
        var fakeQueueUrl = "https://sqs.us-east-1.amazonaws.com/123456789012/checkout";

        int callCount = 0;
        mockSqs.Setup(sqs => sqs.SendMessageAsync(It.IsAny<SendMessageRequest>(), default))
               .Returns(() =>
               {
                   callCount++;
                   if (callCount < 3)
                       throw new Exception("Erro simulado");
                   return Task.FromResult(new SendMessageResponse { MessageId = "12345" });
               });

        var sender = new SqsMessageSender(mockSqs.Object, mockConfig.Object);
        var mensagem = new { Nome = "Retry", Quantidade = 1 };

        // Act
        await sender.EnviarMensagemAsync(mensagem, fakeQueueUrl);

        // Assert
        mockSqs.Verify(sqs => sqs.SendMessageAsync(It.IsAny<SendMessageRequest>(), default), Times.Exactly(3));
    }
}
