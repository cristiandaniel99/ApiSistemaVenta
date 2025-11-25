using SistemaVenta.DAL.DBContext;
using SistemaVenta.DAL.Repositories.Contract;
using SistemaVenta.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.DAL.Repositories
{
   public class VentaRepository: GenericRepository<Venta> , IVentaRepository
    {
        private readonly DbventaContext _dbContext;

        public VentaRepository(DbventaContext dbContext) : base(dbContext) //como estamos usando otra implementacion que tambien necesita el contexto a la base de datos
        {
            _dbContext = dbContext;
        }

        public async Task<Venta> Registrar(Venta modelo)
        {
            Venta ventaGenerada = new Venta();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (DetalleVenta dv in modelo.DetalleVenta)
                    {
                        Producto producto_encontrado = _dbContext.Productos.Where(p => p.IdProducto == dv.IdProducto).First();

                        producto_encontrado.Stock = producto_encontrado.Stock - dv.Cantidad; 

                        _dbContext.Productos.Update(producto_encontrado);


                    }

                    await _dbContext.SaveChangesAsync();

                    NumeroDocumento correlativo = _dbContext.NumeroDocumentos.First();

                    correlativo.UltimoNumero = correlativo.UltimoNumero + 1;

                    correlativo.FechaRegistro = DateTime.Now;


                    _dbContext.NumeroDocumentos.Update(correlativo);

                    await _dbContext.SaveChangesAsync();

                    

                    int CantidadDigitos = 4;
                    string ceros = string.Concat(Enumerable.Repeat("0", CantidadDigitos)); //repite la cantidad de ceros

                    string numeroVenta = ceros + correlativo.UltimoNumero.ToString();

                    //00001

                    numeroVenta = numeroVenta.Substring(numeroVenta.Length - CantidadDigitos, CantidadDigitos);
                    // quedaria 0001 



                    modelo.NumeroDocumento = numeroVenta;


                    await _dbContext.AddAsync(modelo);

                    await _dbContext.SaveChangesAsync();

                    ventaGenerada = modelo;

                    transaction.Commit();



                }




                catch
                {
                    transaction.Rollback();

                    throw;  

                }

                return ventaGenerada;


            }

        }
    }
}
