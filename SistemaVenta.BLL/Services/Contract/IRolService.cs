using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVenta.DTO;

namespace SistemaVenta.BLL.Contract
{
    public interface IRolService
    {

        Task<List<RolDTO>> Lista();
    }
}
