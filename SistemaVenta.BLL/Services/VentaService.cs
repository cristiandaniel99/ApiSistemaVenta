using MapsterMapper;
using Microsoft.EntityFrameworkCore;
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
    public class VentaService : IVentaService
    {
        private readonly IVentaRepository _ventaRepository;

        private readonly IGenericRepository<DetalleVenta> _detalleVentaRepository;

        private readonly IMapper _mapper;

        public VentaService(IVentaRepository ventaRepository, IGenericRepository<DetalleVenta> detalleVentaRepository, IMapper mapper)
        {
            _ventaRepository = ventaRepository;
            _detalleVentaRepository = detalleVentaRepository;
            _mapper = mapper;
        }

        public async Task<List<VentaDTO>> Historial(string buscarPor, string numeroVenta, string fechaInicio, string fechaFin)
        {
            IQueryable<Venta> query = await _ventaRepository.Consultar();
            var listaResultado = new List<Venta>();

            try
            {

                if (buscarPor == "fecha") { 

                    DateTime fech_inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-AR"));
                    DateTime fech_fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-AR"));

                    listaResultado = await query.Where(v=> v.FechaRegistro.Value.Date >= fech_inicio.Date && v.FechaRegistro.Value.Date <= fech_fin.Date).Include(dv=>dv.DetalleVenta).ThenInclude(p=>p.IdProductoNavigation).ToListAsync();
                }
                else
                {
                    listaResultado = await query.Where(v => v.NumeroDocumento == numeroVenta).Include(dv => dv.DetalleVenta).ThenInclude(p => p.IdProductoNavigation).ToListAsync();
                }


            }

            catch 
            {
                throw;
            }

            return _mapper.Map<List<VentaDTO>>(listaResultado);
        }

        public async Task<VentaDTO> Registrar(VentaDTO modelo)
        {
            try
            {

                var ventaGenerada = await _ventaRepository.Registrar(_mapper.Map<Venta>(modelo));

                if (ventaGenerada.IdVenta == 0)
                    throw new TaskCanceledException("No se pudo registrar la venta");

                return _mapper.Map<VentaDTO>(ventaGenerada);

            }

            catch 
            {
                throw;
            }
        }

        public async Task<List<ReporteDTO>> Reporte(string fechaInicio, string fechaFin)
        {

            IQueryable<DetalleVenta> query = await _detalleVentaRepository.Consultar();
            var listaResultado = new List<DetalleVenta>();
            try
            {
                DateTime fech_inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-AR"));
                DateTime fech_fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-AR"));


                listaResultado = await query.Include(p => p.IdProductoNavigation)
                    .Include(v => v.IdVentaNavigation).Where(dv => dv.IdVentaNavigation.FechaRegistro.Value.Date >= fech_inicio.Date && dv.IdVentaNavigation.FechaRegistro.Value.Date <= fech_fin.Date)
                    .ToListAsync();
            }
            catch 
            {
                throw;
            }


            return _mapper.Map<List<ReporteDTO>>(listaResultado);
        }
    }
}