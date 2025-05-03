using FoodOrder.Pedidos.Application.DTOs.Pedidos;
using FoodOrder.Pedidos.Application.UseCase.Pedidos.Interface;
using MediatR;

namespace FoodOrder.Pedidos.Application.Feature.Pedidos
{
    public class PedidosCollectionQuery : IRequest<PedidosOutput>
    {
        
    }

    public class PedidosCollectionQueryHandler : IRequestHandler<PedidosCollectionQuery, PedidosOutput>
    {
        private readonly IPedidoUseCase _pedidoUseCase;

        public PedidosCollectionQueryHandler(IPedidoUseCase pedidoUseCase)
        {
            _pedidoUseCase = pedidoUseCase;
        }

        public async Task<PedidosOutput> Handle(PedidosCollectionQuery request, CancellationToken cancellationToken)
        {
            return await _pedidoUseCase.ListarPedidos();
        }
    }
}
