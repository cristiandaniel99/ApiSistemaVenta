using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SistemaVenta.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistemaVenta.DAL.Repositories.Contract;
using SistemaVenta.DAL.Repositories;
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
        }
    }
}
