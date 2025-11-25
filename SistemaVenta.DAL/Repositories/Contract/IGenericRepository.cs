using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace SistemaVenta.DAL.Repositories.Contract
{
    public interface IGenericRepository<TModel> where TModel : class //trabajar de manera generica con todos los modelos
    {


        Task<TModel> Obtener(Expression<Func<TModel, bool>> filtro);
        Task<TModel> Crear(TModel modelo);

        Task<bool> Editar (TModel modelo);

        Task<bool> Eliminar(TModel modelo);

        Task<IQueryable<TModel>> Consultar(Expression<Func<TModel, bool>> filtro = null);

    }
}
