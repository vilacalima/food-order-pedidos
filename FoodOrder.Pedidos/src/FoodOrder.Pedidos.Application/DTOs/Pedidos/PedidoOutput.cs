using FoodOrder.Pedidos.Application.DTOs.PedidoStatus;
using FoodOrder.Pedidos.Application.DTOs.Produto;

namespace FoodOrder.Pedidos.Application.DTOs.Pedidos
{
    public class PedidoOutput
    {
        public int Id { get; set; }

        public int NumeroPedido { get; set; }

        public TimeSpan TempoEspera { get; set; }

        public DateTime DataCriacao { get; set; }

        public Guid? ClienteId { get; set; }

        public int? PagamentoId { get; set; }

        public PedidoStatusOutput? PedidoStatus { get; private set; }

        public int? SacolaId { get; set; }

        public List<ProdutoOutput> Produtos { get; set; } = [];

        public void SetPedidoStatus(PedidoStatusOutput pedidoStatus) => PedidoStatus = pedidoStatus;
    }
}
