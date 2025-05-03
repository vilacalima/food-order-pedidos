using System.ComponentModel;

namespace FoodOrder.Pedidos.Domain.Enums
{
    public enum PedidoStatusEnum
    {
        [Description("Recebido")]
        Recebido,
        [Description("Em Preparação")]
        EmPreparacao,
        [Description("Pronto")]
        Pronto,
        [Description("Finalizado")]
        Finalizado,
        [Description("Cancelado")]
        Cancelado
    }
}
