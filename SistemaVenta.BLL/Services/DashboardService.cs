using MapsterMapper;
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
    public class DashboardService : IDashboardService
    {

        private readonly IVentaRepository _ventaRepository;

        private readonly IGenericRepository<Producto> _productoRepository;

        private readonly IMapper _mapper;

        public DashboardService(IVentaRepository ventaRepository, IGenericRepository<Producto> productoRepository, IMapper mapper)
        {
            _ventaRepository = ventaRepository;
            _productoRepository = productoRepository;
            _mapper = mapper;
        }

        private IQueryable<Venta> retornarVenta(IQueryable<Venta> tablaVenta, int restarCantidadDias) {

            DateTime? ultimaFecha = tablaVenta.OrderByDescending(v => v.FechaRegistro).Select(v => v.FechaRegistro).First();

            ultimaFecha = ultimaFecha.Value.AddDays(restarCantidadDias);

            return tablaVenta.Where(v => v.FechaRegistro.Value.Date == ultimaFecha.Value.Date);

        }

        public Task<DashboardDTO> resumen()
        {
           try
            {

            }
            catch
            {
                throw;
            }
        }
    }
}
