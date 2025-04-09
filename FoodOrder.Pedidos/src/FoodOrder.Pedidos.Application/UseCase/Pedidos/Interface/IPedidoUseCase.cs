using FoodOrder.Pedidos.Application.DTOs.Pedidos;
using FoodOrder.Pedidos.Application.DTOs.PedidoStatus;

namespace FoodOrder.Pedidos.Application.UseCase.Pedidos.Interface
{
    public interface IPedidoUseCase
    {
        Task<PedidosOutput> ListarPedidos();
        Task<PedidoOutput> Consultar(int numeroPedido);
        Task<PedidoStatusOutput> ConsultarStatus(string status);
        Task Atualizar(PedidoOutput pedido);
    }
}
