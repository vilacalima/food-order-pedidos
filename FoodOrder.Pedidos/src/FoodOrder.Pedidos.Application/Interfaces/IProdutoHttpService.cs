using FoodOrder.Pedidos.Application.DTOs.Produto;

namespace FoodOrder.Pedidos.Application.Interfaces
{
    public interface IProdutoHttpService
    {
        Task<ProdutoDto?> ObterProdutoPorIdAsync(int id);
    }
}
