using FoodOrder.Pedidos.Application.DTOs.Pedidos;
using FoodOrder.Pedidos.Application.UseCase.Pedidos.Interface;
using FoodOrder.Pedidos.Domain.Enums;
using FoodOrder.Pedidos.Domain.Messaging;
using MediatR;

namespace FoodOrder.Pedidos.Application.Feature.Checkout;

public class CheckoutCommand : IRequest<PedidoDto>
{
    public Guid ClienteId { get; set; }
    public MetodoPagamento MetodoPagamento { get; set; }
    public List<int> Produtos { get; set; } = [];
}

public class CheckoutCommandHandler(IPedidoUseCase pedidoUseCase, ISqsMessageSender sqs) : IRequestHandler<CheckoutCommand, PedidoDto>
{
    private readonly IPedidoUseCase _pedidoUseCase = pedidoUseCase;
    private readonly ISqsMessageSender _sqs = sqs;

    public async Task<PedidoDto> Handle(CheckoutCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var novoPedido = await _pedidoUseCase.CriarNovoPedido(request.Produtos, request.ClienteId);
            novoPedido.SetMetodoPagamento(request.MetodoPagamento);

            await EnviarPedidoParaPagamento(novoPedido);

            return novoPedido;
        }
        catch (Exception)
        {
            throw;
        }
    }

    private async Task EnviarPedidoParaPagamento(PedidoDto novoPedido)
    {
        await _sqs.EnviarMensagemAsync(novoPedido, "checkout");
    }
}
