namespace FoodOrder.Pedidos.Application.Interfaces
{
    public interface ISqsMessageSender
    {
        Task EnviarMensagemAsync<T>(T mensagem, string queueUrl);
    }
}
