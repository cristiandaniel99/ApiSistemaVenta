using SistemaVenta.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.BLL.Services.Contract
{
   public interface ICategoriaService


    {

        Task<List<CategoriaDTO>> Lista();
    }
}
