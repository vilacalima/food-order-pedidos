using FoodOrder.Pedidos.Domain.Entities;
using FoodOrder.Pedidos.Domain.Repository;
using FoodOrder.Pedidos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var pedidoExistente = await ConsultarPedidoPorNumero(pedido.NumeroPedido) ?? throw new KeyNotFoundException($"Pedido com número {pedido.NumeroPedido} não encontrado.");
            
            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync();
        }
    }
}
