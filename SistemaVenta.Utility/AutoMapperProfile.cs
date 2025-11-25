using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SistemaVenta.DTO;
using SistemaVenta.Model;
using System.Globalization;
namespace SistemaVenta.Utility
{
   public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {

            #region Rol
            CreateMap<Rol, RolDTO>().ReverseMap();  //reverse map sirve para mapear al reves tmb



            #endregion Rol

            #region Menu
            CreateMap<Menu, MenuDTO>().ReverseMap();  //reverse map sirve para mapear al reves tmb



            #endregion Menu

            #region Usuario
            CreateMap<Usuario, UsuarioDTO>().ForMember(destino => destino.RolDescripcion, opt => opt.MapFrom(origen => origen.IdRolNavigation.Nombre)
            ).ForMember(destino => destino.EsActivo, opt => opt.MapFrom(origen => origen.EsActivo == true ? 1 : 0)  // si es true envia 1 sino 0
            );



            CreateMap<Usuario, SessionDTO>()
                .ForMember(destino => destino.RolDescripcion, opt => opt.MapFrom(origen => origen.IdRolNavigation.Nombre));


            CreateMap<UsuarioDTO, Usuario>()
                .ForMember(destino => destino.IdRolNavigation, opt => opt.Ignore() // ignorar esta propiedad para evitar errores
            ).ForMember(destino => destino.EsActivo, opt => opt.MapFrom(origen => origen.EsActivo == 1 ? true : false)  // es la inversion del mapeo anterior
            );

            #endregion Usuario


            #region Categoria
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();  //reverse map sirve para mapear al reves tmb

            #endregion Categoria


            #region Producto
            CreateMap<Producto, ProductoDTO>().ForMember(destino => destino.DescripcionCategoria, opt => opt.MapFrom(origen => origen.IdCategoriaNavigation.Nombre)
            ).ForMember(destino => destino.Precio, opt => opt.MapFrom(origen => Convert.ToString(origen.Precio.Value, new CultureInfo("es-AR"))
       )).ForMember(destino => destino.EsActivo, opt => opt.MapFrom(origen => origen.EsActivo == true ? 1 : 0)  // si es true envia 1 sino 0
            );



            CreateMap<ProductoDTO, Producto>().ForMember(destino => destino.IdCategoriaNavigation, opt => opt.Ignore(
           )).ForMember(destino => destino.Precio, opt => opt.MapFrom(origen => Convert.ToDecimal(origen.Precio, new CultureInfo("es-AR")))
      ).ForMember(destino => destino.EsActivo, opt => opt.MapFrom(origen => origen.EsActivo == 1 ? true : false)  
           );

            #endregion Producto



            #region Venta
            CreateMap<Venta, VentaDTO>().ForMember(destino => destino.TotalTexto, opt => opt.MapFrom(origen => Convert.ToString(origen.Total.Value, new CultureInfo("es-AR"))))
                .ForMember(destino => destino.FechaRegistro, opt => opt.MapFrom(origen => origen.FechaRegistro.Value.ToString("dd/MM/yyyy")));




            CreateMap<VentaDTO, Venta>().ForMember(destino => destino.Total, opt => opt.MapFrom(origen => Convert.ToDecimal(origen.TotalTexto, new CultureInfo("es-AR"))));
            #endregion Venta


            #region DetalleVenta
            CreateMap<DetalleVenta, DetalleVentaDTO>().ForMember(destino => destino.DescripcionProducto, opt => opt.MapFrom(origen => origen.IdProductoNavigation.Nombre)
            ).ForMember(destino => destino.PrecioTexto, opt => opt.MapFrom(origen => Convert.ToString(origen.Precio.Value, new CultureInfo("es-AR"))))
            .ForMember(destino => destino.TotalTexto, opt => opt.MapFrom(origen => Convert.ToString(origen.Total.Value, new CultureInfo("es-AR"))
            ));


            CreateMap<DetalleVentaDTO, DetalleVenta>().ForMember (destino => destino.Precio, opt => opt.MapFrom(origen => Convert.ToDecimal(origen.PrecioTexto, new CultureInfo("es-AR")))
            ).ForMember(destino => destino.Total, opt => opt.MapFrom(origen => Convert.ToDecimal(origen.TotalTexto, new CultureInfo("es-AR")))
            );
            #endregion DetalleVenta

        }




    }
}
