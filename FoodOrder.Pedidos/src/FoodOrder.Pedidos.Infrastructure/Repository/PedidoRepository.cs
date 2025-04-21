using FoodOrder.Pedidos.Domain.Entities;
using FoodOrder.Pedidos.Domain.Enums;
using FoodOrder.Pedidos.Domain.Repository;
using FoodOrder.Pedidos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FoodOrder.Pedidos.Infrastructure.Repository
{
    public class PedidoRepository(PedidosDbContext context) : IPedidoRepository
    {
        private readonly PedidosDbContext _context = context;

        public async Task<int> Cadastrar(Pedido pedido)
        {
            if (pedido == null) throw new ArgumentNullException(nameof(pedido));

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();
            return pedido.NumeroPedido;
        }

        public async Task<List<Pedido>> ListarPedidos()
        {
            return await _context.Pedidos.ToListAsync();
        }

        public async Task<Pedido?> ConsultarPedidoPorNumero(int numeroPedido)
        {
            return await _context.Pedidos.FirstOrDefaultAsync(x => x.NumeroPedido == numeroPedido);
        }

        public async Task Atualizar(Pedido pedido)
        {
            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarStatusPagamento(int id, PagamentoStatusEnum pagamento)
        {
            var pedido = await _context.Pedidos.FindAsync(id) ?? throw new InvalidOperationException($"Pedido with ID {id} not found.");
            pedido.PagamentoStatus = pagamento;
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarStatusPedido(int id, PedidoStatusEnum pedidoStatus)
        {
            var pedido = await _context.Pedidos.FindAsync(id) ?? throw new InvalidOperationException($"Pedido with ID {id} not found.");
            pedido.PedidoStatus = pedidoStatus;
            await _context.SaveChangesAsync();
        }
    }
}
