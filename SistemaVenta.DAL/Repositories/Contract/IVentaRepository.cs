using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistemaVenta.Model;

namespace SistemaVenta.DAL.Repositories.Contract
{
    public interface IVentaRepository : IGenericRepository<Venta>
    {

        Task<Venta> Registrar(Venta modelo);



    }
}
