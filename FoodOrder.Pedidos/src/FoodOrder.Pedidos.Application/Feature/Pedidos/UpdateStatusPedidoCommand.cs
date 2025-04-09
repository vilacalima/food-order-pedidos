using FoodOrder.Pedidos.Application.DTOs.Pedidos;
using FoodOrder.Pedidos.Application.DTOs.PedidoStatus;
using FoodOrder.Pedidos.Application.UseCase.Pedidos.Interface;
using MediatR;

namespace FoodOrder.Pedidos.Application.Feature.Pedidos
{
    public class UpdateStatusPedidoCommand(int numeroPedido, string status) : IRequest<Unit>
    {
        public int NumeroPedido { get; set; } = numeroPedido;
        public string Status { get; set; } = status;
    }

    public class UpdateStatusPedidoCommandHandler : IRequestHandler<UpdateStatusPedidoCommand, Unit>
    {
        private readonly IPedidoUseCase _pedidoUseCase;

        public UpdateStatusPedidoCommandHandler(IPedidoUseCase pedidoUseCase)
        {
            _pedidoUseCase = pedidoUseCase;
        }

        public async Task<Unit> Handle(UpdateStatusPedidoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                PedidoOutput pedido = await ConsultarPedido(request);
                PedidoStatusOutput status = await ConsultaStatus(request);

                pedido.SetPedidoStatus(status);

                await _pedidoUseCase.Atualizar(pedido);

                return Unit.Value;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<PedidoStatusOutput> ConsultaStatus(UpdateStatusPedidoCommand request)
        {
            var status = await _pedidoUseCase.ConsultarStatus(request.Status);

            if (status == null) throw new KeyNotFoundException("Pedido não encontrado!");

            return status;
        }

        private async Task<PedidoOutput> ConsultarPedido(UpdateStatusPedidoCommand request)
        {
            var pedido = await _pedidoUseCase.Consultar(request.NumeroPedido);

            if (pedido == null) throw new KeyNotFoundException("Pedido não encontrado!");

            return pedido;
        }
    }
}
