using FoodOrder.Pedidos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrder.Pedidos.Domain.Repository
{
    public interface IPedidoStatusRepository
    {
        Task<PedidoStatus> Cadastrar(PedidoStatus pedidoStatus);

        Task<PedidoStatus?> ConsultarPorId(int id);
    }
}
