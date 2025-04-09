using FoodOrder.Pedidos.Domain.Entities;

namespace FoodOrder.Pedidos.Domain.Repository
{
    public interface ISacolaProdutoRepository
    {
        Task<SacolaProduto> Cadastrar(SacolaProduto produto);
        Task<List<SacolaProduto>> ConsultarPorSacola(int id);
    }
}
