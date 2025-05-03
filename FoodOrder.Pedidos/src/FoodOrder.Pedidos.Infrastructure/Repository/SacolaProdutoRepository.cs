using FoodOrder.Pedidos.Domain.Entities;
using FoodOrder.Pedidos.Domain.Repository;
using FoodOrder.Pedidos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FoodOrder.Pedidos.Infrastructure.Repository
{
    public class SacolaProdutoRepository(PedidosDbContext context) : ISacolaProdutoRepository
    {
        private readonly PedidosDbContext _context = context;

        public async Task<List<SacolaProduto>> ConsultarPorSacola(int id)
        {
            var sacolaProduto = await _context.SacolasProdutos
               .Where(x => x.SacolaId == id)
               .ToListAsync();

            return sacolaProduto;
        }

        public async Task<SacolaProduto> Cadastrar(SacolaProduto sacolaProduto)
        {
            if (sacolaProduto == null) throw new ArgumentNullException(nameof(sacolaProduto));

            _context.SacolasProdutos.Add(sacolaProduto);
            await _context.SaveChangesAsync();
            return sacolaProduto;
        }
    }
}
