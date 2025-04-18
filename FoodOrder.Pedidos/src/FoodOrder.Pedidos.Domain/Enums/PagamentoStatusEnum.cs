using System.ComponentModel;

namespace FoodOrder.Pedidos.Domain.Enums
{
    public enum PagamentoStatusEnum
    {
        [Description("Pagamento Realizado")]
        PagamentoRealizado,
        [Description("Pagamento Rejeitado")]
        PagamentoRejeitado,
        [Description("Aguardando Pagamento")]
        AguardandoPagamento
    }
}
