using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.BLL.Services.Contract;
using SistemaVenta.DAL.Repositories.Contract;
using SistemaVenta.DTO;
using SistemaVenta.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.BLL.Services
{
    public class ProductoService : IProductoService
    {


        private readonly IGenericRepository<Producto> _productoRepository;

        private readonly IMapper _mapper;

        public ProductoService(IGenericRepository<Producto> productoRepostory, IMapper mapper)
        {
            _productoRepository = productoRepostory;
            _mapper = mapper;
        }

        public async Task<List<ProductoDTO>> Lista()
        {
            try
            {
                IQueryable<Producto> query = await _productoRepository.Consultar();

                var listaProductos = query.Include(cat => cat.IdCategoriaNavigation).ToList();

                return _mapper.Map<List<ProductoDTO>>(listaProductos);
            }
            catch
            {
                throw;
            }
        }

        public async Task<ProductoDTO> CrearProducto(ProductoDTO modelo)
        {
            try
            {
                var productoCreado = await _productoRepository.Crear(_mapper.Map<Producto>(modelo));

                if(productoCreado.IdProducto == 0)
                {
                    throw new TaskCanceledException("No se pudo crear el producto");
                }
                return _mapper.Map<ProductoDTO>(productoCreado);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(ProductoDTO modelo)
        {
            try
            {
                var productoModelo = _mapper.Map<Producto>(modelo);
                var productoEncontrado = await _productoRepository.Obtener(p => p.IdProducto == productoModelo.IdProducto);
                
                if(productoEncontrado == null)
                {
                    throw new TaskCanceledException("No se encontro el producto");
                }
                productoEncontrado.Nombre = productoModelo.Nombre;
                productoEncontrado.IdCategoria = productoModelo.IdCategoria;
                productoEncontrado.Precio = productoModelo.Precio;
                productoEncontrado.Stock = productoModelo.Stock;
                productoEncontrado.EsActivo = productoModelo.EsActivo;
                
                bool respuesta = _productoRepository.Editar(productoEncontrado).Result;

                if(!respuesta) throw new TaskCanceledException("No se pudo editar el producto"); ;
                

                return respuesta;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(int idProducto)
        {
            try
            {
                var productoEncontrado = await _productoRepository.Obtener(p => p.IdProducto == idProducto);

                if(productoEncontrado == null)
                {
                    throw new TaskCanceledException("No se encontro el producto");
                }
                bool respuesta = _productoRepository.Eliminar(productoEncontrado).Result;

                if(!respuesta) throw new TaskCanceledException("No se pudo eliminar el producto"); ;

                return respuesta;
            }
            catch
            {
                throw;
            }
        }

       
    }
}
