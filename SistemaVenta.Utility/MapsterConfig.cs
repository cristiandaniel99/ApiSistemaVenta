using Mapster;
using MapsterMapper;
using SistemaVenta.DTO;
using SistemaVenta.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.Utility
{
    public class MapsterConfig : IRegister
    {


        public void Register(TypeAdapterConfig config)
        {
            // Definimos la cultura globalmente para este archivo
            var culturaArg = new CultureInfo("es-AR");

            #region Rol
            config.NewConfig<Rol, RolDTO>()
                .TwoWays(); // Equivale a ReverseMap()
            #endregion Rol

            #region Menu
            config.NewConfig<Menu, MenuDTO>()
                .TwoWays();
            #endregion Menu

            #region Usuario
            config.NewConfig<Usuario, UsuarioDTO>()
                .Map(dest => dest.RolDescripcion, src => src.IdRolNavigation.Nombre)
                .Map(dest => dest.EsActivo, src => src.EsActivo == true ? 1 : 0);

            config.NewConfig<Usuario, SessionDTO>()
                .Map(dest => dest.RolDescripcion, src => src.IdRolNavigation.Nombre);

            config.NewConfig<UsuarioDTO, Usuario>()
                .Ignore(dest => dest.IdRolNavigation) // Ignoramos la navegación
                .Map(dest => dest.EsActivo, src => src.EsActivo == 1); // True si es 1, False si no
            #endregion Usuario

            #region Categoria
            config.NewConfig<Categoria, CategoriaDTO>()
                .TwoWays();
            #endregion Categoria

            #region Producto
            config.NewConfig<Producto, ProductoDTO>()
                .Map(dest => dest.DescripcionCategoria, src => src.IdCategoriaNavigation.Nombre)
                // Usamos "N2" para que siempre tenga 2 decimales (ej: 1500,50)
                .Map(dest => dest.Precio, src => src.Precio.HasValue ? src.Precio.Value.ToString("N2", culturaArg) : "0,00")
                .Map(dest => dest.EsActivo, src => src.EsActivo == true ? 1 : 0);

            config.NewConfig<ProductoDTO, Producto>()
                .Ignore(dest => dest.IdCategoriaNavigation)
                .Map(dest => dest.Precio, src => Convert.ToDecimal(src.Precio, culturaArg))
                .Map(dest => dest.EsActivo, src => src.EsActivo == 1);
            #endregion Producto

            #region Venta
            config.NewConfig<Venta, VentaDTO>()
                .Map(dest => dest.TotalTexto, src => src.Total.HasValue ? src.Total.Value.ToString("N2", culturaArg) : "0,00")
                .Map(dest => dest.FechaRegistro, src => src.FechaRegistro.HasValue ? src.FechaRegistro.Value.ToString("dd/MM/yyyy") : null);

            config.NewConfig<VentaDTO, Venta>()
                .Map(dest => dest.Total, src => Convert.ToDecimal(src.TotalTexto, culturaArg));
            #endregion Venta

            #region DetalleVenta
            config.NewConfig<DetalleVenta, DetalleVentaDTO>()
                .Map(dest => dest.DescripcionProducto, src => src.IdProductoNavigation.Nombre)
                .Map(dest => dest.PrecioTexto, src => src.Precio.HasValue ? src.Precio.Value.ToString("N2", culturaArg) : "0,00")
                .Map(dest => dest.TotalTexto, src => src.Total.HasValue ? src.Total.Value.ToString("N2", culturaArg) : "0,00");

            config.NewConfig<DetalleVentaDTO, DetalleVenta>()
                .Map(dest => dest.Precio, src => Convert.ToDecimal(src.PrecioTexto, culturaArg))
                .Map(dest => dest.Total, src => Convert.ToDecimal(src.TotalTexto, culturaArg));
            #endregion DetalleVenta

            #region Reporte
            // Este es un mapeo complejo "aplanando" datos de tablas relacionadas
            config.NewConfig<DetalleVenta, ReporteDTO>()
                .Map(dest => dest.FechaRegistro, src => src.IdVentaNavigation.FechaRegistro.Value.ToString("dd/MM/yyyy"))
                .Map(dest => dest.NumeroDocumento, src => src.IdVentaNavigation.NumeroDocumento)
                .Map(dest => dest.TipoPago, src => src.IdVentaNavigation.TipoPago)
                .Map(dest => dest.TotalVenta, src => src.IdVentaNavigation.Total.HasValue ? src.IdVentaNavigation.Total.Value.ToString("N2", culturaArg) : "0,00")
                .Map(dest => dest.Producto, src => src.IdProductoNavigation.Nombre)
                .Map(dest => dest.Precio, src => src.Precio.HasValue ? src.Precio.Value.ToString("N2", culturaArg) : "0,00")
                .Map(dest => dest.Total, src => src.Total.HasValue ? src.Total.Value.ToString("N2", culturaArg) : "0,00");
            #endregion Reporte
        }




    }
}
