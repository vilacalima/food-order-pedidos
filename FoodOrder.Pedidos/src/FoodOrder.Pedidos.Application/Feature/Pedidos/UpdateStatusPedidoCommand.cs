using FoodOrder.Pedidos.Application.DTOs.Pedidos;
using FoodOrder.Pedidos.Application.UseCase.Pedidos.Interface;
using FoodOrder.Pedidos.Domain.Enums;
using MediatR;

namespace FoodOrder.Pedidos.Application.Feature.Pedidos;

public class UpdateStatusPedidoCommand(int numeroPedido, PedidoStatusEnum status) : IRequest<Unit>
{
    public int NumeroPedido { get; set; } = numeroPedido;
    public PedidoStatusEnum Status { get; set; } = status;
}

public class UpdateStatusPedidoCommandHandler(IPedidoUseCase pedidoUseCase) : IRequestHandler<UpdateStatusPedidoCommand, Unit>
{
    private readonly IPedidoUseCase _pedidoUseCase = pedidoUseCase;

    public async Task<Unit> Handle(UpdateStatusPedidoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            PedidoOutput pedido = await ConsultarPedido(request);

            pedido.SetPedidoStatus(request.Status);

            await _pedidoUseCase.AtualizarStatusPedido(pedido);
            
            return Unit.Value;
        }
        catch (Exception)
        {
            throw;
        }
    }

    private async Task<PedidoOutput> ConsultarPedido(UpdateStatusPedidoCommand request)
    {
        var pedido = await _pedidoUseCase.Consultar(request.NumeroPedido);

        return pedido ?? throw new KeyNotFoundException("Pedido não encontrado!");
    }
}
