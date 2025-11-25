using SistemaVenta.DAL.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistemaVenta.Model;
using SistemaVenta.DAL.DBContext;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace SistemaVenta.DAL.Repositories
{
    public class GenericRepository<TModel> : IGenericRepository<TModel> where TModel : class
    {

        private readonly DbventaContext _dbcontext;

        public GenericRepository(DbventaContext dbcontext)
        {
            _dbcontext = dbcontext;
        }


        public async Task<TModel> Obtener(Expression<Func<TModel, bool>> filtro) //el filtro busca por el filtro
        {
            try { 
            
                TModel model = await _dbcontext.Set<TModel>().FirstOrDefaultAsync(filtro);
                return model;
               
            }
            catch {

                throw;
            
            }
        }

        public async Task<TModel> Crear(TModel modelo)
        {
            try
            {

                _dbcontext.Set<TModel>().Add(modelo);

                await _dbcontext.SaveChangesAsync();

                return modelo;

            }
            catch
            {

                throw;

            }
        }

        public async Task<bool> Editar(TModel modelo)
        {
            try
            {
                _dbcontext.Set<TModel>().Update(modelo);

                await _dbcontext.SaveChangesAsync();

                return true;
                   


            }
            catch
            {

                throw;

            }
        }

        public async Task<bool> Eliminar(TModel modelo)
        {
            try
            {

                _dbcontext.Set<TModel>().Remove(modelo);

               await _dbcontext.SaveChangesAsync();


                return true;
               

            }
            catch
            {

                throw;

            }
        }

        public async Task<IQueryable<TModel>> Consultar(Expression<Func<TModel, bool>> filtro = null)
        {
            try
            {

                IQueryable<TModel> queryModel = filtro == null? _dbcontext.Set<TModel>() : _dbcontext.Set<TModel>().Where(filtro); // si el filtro es nulo retorna el modelo, en el caso contrario retorna el modelo que especifica el filtro
                return queryModel;

            }
            catch
            {

                throw;

            }
        }

       

        
    }
}
