using FoodOrder.Pedidos.Application.DTOs.Pedidos;
using FoodOrder.Pedidos.Application.DTOs.PedidoStatus;

namespace FoodOrder.Pedidos.Application.UseCase.Pedidos.Interface
{
    public interface IPedidoUseCase
    {
        Task<PedidosOutput> ListarPedidos();
        Task<PedidoOutput> Consultar(int numeroPedido);
        Task Atualizar(PedidoOutput pedido);
        Task<PedidoDto> CriarNovoPedido(List<int> produtos, Guid ClienteId);
        Task AtualizarStatusPagamento(PedidoOutput pedidoAtualizado);
        Task AtualizarStatusPedido(PedidoOutput pedidoAtualizado);
    }
}
