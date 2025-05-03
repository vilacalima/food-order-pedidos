using FoodOrder.Pedidos.Domain.Entities;
using FoodOrder.Pedidos.Domain.Enums;

namespace FoodOrder.Pedidos.Domain.Repository
{
    public interface IPedidoRepository
    {
        Task<int> Cadastrar(Pedido pedido);
        Task<List<Pedido>> ListarPedidos();
        Task<Pedido?> ConsultarPedidoPorNumero(int numeroPedido);
        Task Atualizar(Pedido pedido);
        Task AtualizarStatusPagamento(int id, PagamentoStatusEnum pagamento);
        Task AtualizarStatusPedido(int id, PedidoStatusEnum pedidoStatus);
    }
}
