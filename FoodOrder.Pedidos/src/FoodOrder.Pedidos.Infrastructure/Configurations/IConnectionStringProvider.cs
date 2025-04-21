using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrder.Pedidos.Infrastructure.Configurations
{
    public interface IConnectionStringProvider
    {
        string GetConnectionString(string name);
    }
}
