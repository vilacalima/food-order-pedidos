using FoodOrder.Pedidos.Application.DTOs.Produto;
using FoodOrder.Pedidos.Domain.Enums;

namespace FoodOrder.Pedidos.Application.DTOs.Pedidos;

public class PedidoOutput
{
    public int Id { get; set; }

    public int NumeroPedido { get; set; }

    public TimeSpan TempoEspera { get; set; }

    public DateTime DataCriacao { get; set; }

    public Guid? ClienteId { get; set; }

    public PedidoStatusEnum PedidoStatus { get; set; }

    public PagamentoStatusEnum PagamentoStatus { get; set; }

    public int? SacolaId { get; set; }

    public List<ProdutoOutput> Produtos { get; set; } = [];

    public void SetPedidoStatus(PedidoStatusEnum pedidoStatus) => PedidoStatus = pedidoStatus;
    public void SetPagamentoStatus(PagamentoStatusEnum status) => PagamentoStatus = status;
}
