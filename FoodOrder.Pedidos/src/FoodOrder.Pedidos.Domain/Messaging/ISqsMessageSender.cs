using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrder.Pedidos.Domain.Messaging
{
    public interface ISqsMessageSender
    {
        Task EnviarMensagemAsync<T>(T mensagem, string queueUrl);
    }
}
