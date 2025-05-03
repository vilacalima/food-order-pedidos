using FoodOrder.Pedidos.Domain.Entities;

namespace FoodOrder.Pedidos.Domain.Repository
{
    public interface ISacolaRepository
    {
        Task<Sacola> Cadastrar(Sacola sacola);
        Task<Sacola> ResgatarUltimaSacola();
    }
}
