using FoodOrder.Pedidos.Application.DTOs.Produto;

namespace FoodOrder.Pedidos.Application.Mapper
{
    public class ProdutoMapper
    {
        public static ProdutoOutput Map(ProdutoDto produto)
        {
            ProdutoOutput produtoOutput = new()
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Descricao = produto.Descricao
            };

            return produtoOutput;
        }
    }
}
