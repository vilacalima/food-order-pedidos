namespace FoodOrder.Pedidos.Application.DTOs.PedidoStatus
{
    public class PedidoStatusOutput(int id, string descricao)
    {
        public int Id { get; set; } = id;
        public string Descricao { get; set; } = descricao;
    }

}
