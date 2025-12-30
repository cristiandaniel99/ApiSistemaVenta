using SistemaVenta.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.BLL.Services.Contract
{
    public interface IProductoService
    {

        

        Task<List<ProductoDTO>> Lista();
       

        Task<ProductoDTO> CrearProducto(ProductoDTO modelo);

        Task<bool> Editar(ProductoDTO modelo);

        Task<bool> Eliminar(int idProducto);
    }
}
