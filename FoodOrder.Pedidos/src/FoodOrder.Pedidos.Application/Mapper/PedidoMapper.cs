using FoodOrder.Pedidos.Application.DTOs.Pedidos;
using FoodOrder.Pedidos.Application.DTOs.Produto;
using FoodOrder.Pedidos.Domain.Entities;

namespace FoodOrder.Pedidos.Application.Mapper
{
    public class PedidoMapper
    {
        public static PedidoOutput Map(Pedido pedido)
        {
            PedidoOutput pedidoOutput = new PedidoOutput();

            pedidoOutput.Id = pedido.Id;
            pedidoOutput.NumeroPedido = pedido.NumeroPedido;
            pedidoOutput.TempoEspera = pedido.TempoEspera;
            pedidoOutput.DataCriacao = pedido.DataCriacao;
            pedidoOutput.ClienteId = (pedido.ClienteId == Guid.Empty) ? null : pedido.ClienteId;
            pedidoOutput.PagamentoStatus = pedido.PagamentoStatus;
            pedidoOutput.PedidoStatus = pedido.PedidoStatus;
            pedidoOutput.SacolaId = pedido.SacolaId;
            pedidoOutput.Produtos = new List<ProdutoOutput>();

            return pedidoOutput;
        }

        public static Pedido Map(PedidoOutput pedidoOutput, int pedidoId)
        {
            Pedido pedido = new Pedido();

            pedido.Id = pedidoId;
            pedido.NumeroPedido = pedidoOutput.NumeroPedido;
            pedido.TempoEspera = pedidoOutput.TempoEspera;
            pedido.DataCriacao = pedidoOutput.DataCriacao;
            pedido.ClienteId = pedidoOutput.ClienteId ?? Guid.Empty;
            pedido.PedidoStatus = pedidoOutput.PedidoStatus;
            pedido.SacolaId = pedidoOutput.SacolaId ?? 0;
            pedido.PagamentoStatus = pedidoOutput.PagamentoStatus;

            return pedido;
        }
    }
}
