using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SistemaVenta.BLL.Services;
using SistemaVenta.BLL.Services.Contract;
using SistemaVenta.DAL.DBContext;
using SistemaVenta.DAL.Repositories;
using SistemaVenta.DAL.Repositories.Contract;
using SistemaVenta.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMapper = MapsterMapper.IMapper;
using Microsoft.AspNetCore.Identity;
using SistemaVenta.Model;

namespace SistemaVenta.IOC
{
   public static class Dependencia
    {

        public static void IneyectarDependencias(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<DbventaContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("CadenaSql"));
            });


            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>)); //dependencias de genericos
            services.AddScoped<IVentaRepository, VentaRepository>(); // estamos especificando el modelo exacto


           // services.AddAutoMapper(typeof(AutoMapperProfile)); //inyectar el automapper

            // --- CONFIGURACIÓN MAPSTER ---
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(typeof(MapsterConfig).Assembly); // Escanea el proyecto Utility

            services.AddSingleton(config); // Registra la configuración
            services.AddScoped<IMapper, ServiceMapper>(); // Registra la interfaz IMapper

            services.AddScoped<IRolService, RolService>();
            // Registrar el IPasswordHasher<Usuario> requerido por UsuarioService
            services.AddScoped<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();

            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<IProductoService, ProductoService>();
            services.AddScoped<IVentaService, VentaService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IMenuService, MenuService>();

            

        }
    }
}
