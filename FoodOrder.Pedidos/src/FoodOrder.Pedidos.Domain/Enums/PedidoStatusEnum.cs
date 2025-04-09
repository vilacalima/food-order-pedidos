namespace FoodOrder.Pedidos.Domain.Enums
{
    public enum PedidoStatusEnum
    {
        Pronto,
        EmPreparacao,
        Recebido,
        Finalizado,
        Cancelado
    }

    public static class PedidoStatusMapper
    {
        public static PedidoStatusEnum Mapear(string status)
        {
            return status.ToLower() switch
            {
                "pronto" => PedidoStatusEnum.Pronto,
                "em preparacao" => PedidoStatusEnum.EmPreparacao,
                "em preparação" => PedidoStatusEnum.EmPreparacao,
                "recebido" => PedidoStatusEnum.Recebido,
                "finalizado" => PedidoStatusEnum.Finalizado,
                "cancelado" => PedidoStatusEnum.Cancelado,
                _ => throw new ArgumentException("Status inválido!")
            };
        }

        public static string ObterDescricao(PedidoStatusEnum status)
        {
            return status switch
            {
                PedidoStatusEnum.Pronto => "Pronto",
                PedidoStatusEnum.EmPreparacao => "Em preparação",
                PedidoStatusEnum.Recebido => "Recebido",
                PedidoStatusEnum.Finalizado => "Finalizado",
                PedidoStatusEnum.Cancelado => "Cancelado",
                _ => throw new ArgumentException("Status inválido!")
            };
        }
    }
}
