using FoodOrder.Pedidos.Domain.Enums;

namespace FoodOrder.Pedidos.Application.DTOs.Pedidos
{
    public class PedidoDto(int numeroPedido, decimal preco)
    {
        public int NumeroPedido { get; set; } = numeroPedido;
        public decimal Preco { get; set; } = preco;
        public MetodoPagamento MetodoPagamento { get; private set; }

        public void SetMetodoPagamento(MetodoPagamento metodoPagamento) => MetodoPagamento = metodoPagamento;
    }
}
