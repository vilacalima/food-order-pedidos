using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrder.Pedidos.Domain.Entities
{
    public class SacolaProduto
    {
        public SacolaProduto()
        {

        }

        public SacolaProduto(int sacolaId, int produtoId)
        {
            SacolaId = sacolaId;
            ProdutoId = produtoId;
        }


        [Key]
        public int Id { get; set; }

        [ForeignKey("SacolaId")]
        public int SacolaId { get; set; }

        [ForeignKey("ProdutoId")]
        public int ProdutoId { get; set; }
    }
}
