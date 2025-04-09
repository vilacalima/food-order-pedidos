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
            Pronto = new List<PedidoOutput>();
            EmPreparo = new List<PedidoOutput>();
            Recebido = new List<PedidoOutput>();
        }

        public PedidosOutput(List<PedidoOutput> pronto, List<PedidoOutput> emPreparo, List<PedidoOutput> recebido)
        {
            Pronto = pronto;
            EmPreparo = emPreparo;
            Recebido = recebido;
        }

        public List<PedidoOutput> Pronto { get; set; }
        public List<PedidoOutput> EmPreparo { get; set; }
        public List<PedidoOutput> Recebido { get; set; }
    }
}
