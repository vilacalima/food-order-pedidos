using FoodOrder.Pedidos.Domain.Entities;

namespace FoodOrder.Pedidos.Domain.Repository
{
    public interface IPedidoRepository
    {
        Task<int> Cadastrar(Pedido pedido);
        Task<List<Pedido>> ListarPedidos();
        Task<Pedido?> ConsultarPedidoPorNumero(int numeroPedido);
        Task Atualizar(Pedido pedido);
    }
}
