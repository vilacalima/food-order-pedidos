using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrder.Pedidos.Application.DTOs.Produto
{
    public class ProdutoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public decimal Preco { get; set; }
        public string Descricao { get; set; }
        public int TempoPreparo { get; set; }
    }
}
