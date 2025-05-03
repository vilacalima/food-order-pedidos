using FoodOrder.Pedidos.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FoodOrder.Pedidos.Domain.Entities
{
    public class Pedido
    {
        public Pedido()
        {
        }

        public Pedido(TimeSpan tempoEspera, Guid clienteId, PagamentoStatusEnum pagamentoStatus, PedidoStatusEnum pedidoStatus, int sacolaId)
        {
            TempoEspera = tempoEspera;
            DataCriacao = DateTime.UtcNow;
            ClienteId = clienteId;
            PagamentoStatus = pagamentoStatus;
            PedidoStatus = pedidoStatus;
            SacolaId = sacolaId;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public int NumeroPedido { get; set; }

        public TimeSpan TempoEspera { get; set; }

        [JsonIgnore]
        public DateTime DataCriacao { get; set; }

        [ForeignKey("ClienteId")]
        public Guid ClienteId { get; set; }

        [ForeignKey("PagamentoId")]
        public PagamentoStatusEnum PagamentoStatus { get; set; }

        [ForeignKey("PedidoStatusId")]
        public PedidoStatusEnum PedidoStatus { get; set; }

        [ForeignKey("SacolaId")]
        public int SacolaId { get; set; }
    }
}
