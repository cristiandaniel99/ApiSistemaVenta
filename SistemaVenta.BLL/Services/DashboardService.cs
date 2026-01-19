using MapsterMapper;
using Microsoft.IdentityModel.Tokens;
using SistemaVenta.BLL.Services.Contract;
using SistemaVenta.DAL.Repositories.Contract;
using SistemaVenta.DTO;
using SistemaVenta.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        private IQueryable<Venta> retornarVentas(IQueryable<Venta> tablaVenta, int restarCantidadDias) {

            DateTime? ultimaFecha = tablaVenta.OrderByDescending(v => v.FechaRegistro).Select(v => v.FechaRegistro).First();

            ultimaFecha = ultimaFecha.Value.AddDays(restarCantidadDias);

            return tablaVenta.Where(v => v.FechaRegistro.Value.Date == ultimaFecha.Value.Date);


          

        }

        private async Task<int> totalVentasUltimaSemana()
        {
            int total = 0;
            IQueryable<Venta> _ventaQuery = await _ventaRepository.Consultar();

            if(_ventaQuery.Count() > 0)
            {
                var tableVenta = retornarVentas(_ventaQuery, -7);
                total = tableVenta.Count();
            }
            return total;

        }

        private async Task<string> totalIngresosUltimaSemana()
        {
            decimal resultado = 0;
            IQueryable<Venta> _ventaQuery = await _ventaRepository.Consultar();

            if (_ventaQuery.Count() > 0)
            {
                var tablaVenta = retornarVentas(_ventaQuery, -7);

                resultado = tablaVenta.Select(v=>v.Total).Sum(v=>v.Value);

            }
            return Convert.ToString(resultado, new CultureInfo("es-AR"));

        }

        private async Task<int> totalProductos()
        {
            IQueryable<Producto> _productoQuery = await _productoRepository.Consultar();

            int total = _productoQuery.Count();

            return total;
        }

        private async Task<Dictionary<string, int>> ventasUltimaSemana()
        {
            Dictionary<string, int> resultado = new Dictionary<string, int>();

            IQueryable<Venta> _ventaQuery = await _ventaRepository.Consultar();

            if (_ventaQuery.Count() > 0)
            {
                var tablaVenta = retornarVentas(_ventaQuery, -7);

                resultado = tablaVenta
                    .GroupBy(v => v.FechaRegistro.Value.Date).OrderBy(g => g.Key).Select(dv => new { fecha = dv.Key.ToString("dd/MM/yyyy"), total = dv.Count() }).ToDictionary(keySelector: r=> r.fecha, elementSelector: r=>r.total);
            }

            return resultado;

        }




        public async Task<DashboardDTO> resumen()
        {
            DashboardDTO vmDashBoard = new DashboardDTO();
            try
            {
                vmDashBoard.TotalVentas = await totalVentasUltimaSemana();
                vmDashBoard.TotalIngresos = await totalIngresosUltimaSemana();
                vmDashBoard.TotalProducto = await totalProductos();


                List<VentaSemanaDTO> listaVentaSemana = new List<VentaSemanaDTO>();

                foreach(KeyValuePair<string, int> item in await ventasUltimaSemana())
                {
                    listaVentaSemana.Add(new VentaSemanaDTO()
                    {
                        Fecha = item.Key,
                        Total = item.Value
                    });
                }
                vmDashBoard.VentasUltimaSemana = listaVentaSemana;

            }
            catch
            {
                throw;
            }

            return vmDashBoard;
        }
    }
}
