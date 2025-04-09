using FoodOrder.Pedidos.Domain.Entities;
using FoodOrder.Pedidos.Domain.Repository;
using FoodOrder.Pedidos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FoodOrder.Pedidos.Infrastructure.Repository
{
    public class SacolaRepository(PedidosDbContext context) : ISacolaRepository
    {
        private readonly PedidosDbContext _context = context;

        public async Task<Sacola> Cadastrar(Sacola sacola)
        {
            if (sacola == null) throw new ArgumentNullException(nameof(sacola));

            _context.Sacola.Add(sacola);
            await _context.SaveChangesAsync();
            return sacola;
        }

        public async Task<Sacola> ResgatarUltimaSacola()
        {
            var sacola = await _context.Sacola.ToListAsync();

            return sacola.LastOrDefault()
                ?? throw new InvalidOperationException("Nenhuma sacola encontrada.");
        }
    }
}
