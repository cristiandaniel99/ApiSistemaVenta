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

        private readonly IGenericRepository<Rol> _rolRepository;

        private readonly IMapper _mapper;

        public RolService(IGenericRepository<Rol> rolRepository, IMapper mapper)
        {
            _rolRepository = rolRepository;
            _mapper = mapper;
        }

        public async Task<List<RolDTO>> Lista()
        {
            try {
                var listaRoles = await _rolRepository.Consultar();
                return _mapper.Map<List<RolDTO>>(listaRoles.ToList());
            }
            catch
            {
                throw;
            }
            
        }
    }
}
