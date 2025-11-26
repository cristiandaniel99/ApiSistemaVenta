using SistemaVenta.DAL.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVenta.Model;
using MapsterMapper;
using SistemaVenta.BLL.Services.Contract;
using SistemaVenta.DTO;

namespace SistemaVenta.BLL.Services
{
    public class RolService : IRolService
    {

        private readonly IGenericRepository<Rol> _rolRepostory;

        private readonly IMapper _mapper;

        public RolService(IGenericRepository<Rol> rolRepostory, IMapper mapper)
        {
            _rolRepostory = rolRepostory;
            _mapper = mapper;
        }

        public async Task<List<RolDTO>> Lista()
        {
            try {
                var listaRoles = await _rolRepostory.Consultar();
                return _mapper.Map<List<RolDTO>>(listaRoles.ToList());
            }
            catch
            {
                throw;
            }
            ;
        }
    }
}
