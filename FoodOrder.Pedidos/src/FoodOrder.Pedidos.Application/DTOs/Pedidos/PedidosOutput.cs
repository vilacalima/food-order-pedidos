using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrder.Pedidos.Application.DTOs.Pedidos
{
    public class PedidosOutput
    {
        public PedidosOutput() 
        {
            Pronto = [];
            EmPreparacao = [];
            Recebido = [];
            Finalizado = [];
        }

        public PedidosOutput(List<PedidoOutput> pronto, List<PedidoOutput> emPreparo, List<PedidoOutput> recebido, List<PedidoOutput> finalizado)
        {
            Pronto = pronto;
            EmPreparacao = emPreparo;
            Recebido = recebido;
            Finalizado = finalizado;
        }

        public List<PedidoOutput> Pronto { get; set; } 
        public List<PedidoOutput> EmPreparacao { get; set; } 
        public List<PedidoOutput> Recebido { get; set; } 
        public List<PedidoOutput> Finalizado { get; set; } 
    }
}
