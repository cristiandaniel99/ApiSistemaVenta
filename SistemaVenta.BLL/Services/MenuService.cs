using MapsterMapper;
using SistemaVenta.BLL.Services.Contract;
using SistemaVenta.DAL.Repositories.Contract;
using SistemaVenta.DTO;
using SistemaVenta.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.BLL.Services
{
    public class MenuService : IMenuService
    {
        private readonly IGenericRepository<MenuRol> _menuRolRepository;

        private readonly IGenericRepository<Usuario> _usuarioRepository;

        private readonly IGenericRepository<Menu> _menuRepository;

        private readonly IMapper _mapper;

        public MenuService(IGenericRepository<MenuRol> menuRolRepository, IGenericRepository<Usuario> usuarioRepository, IGenericRepository<Menu> menuRepository, IMapper mapper)
        {
            _menuRolRepository = menuRolRepository;
            _usuarioRepository = usuarioRepository;
            _menuRepository = menuRepository;
            _mapper = mapper;
        }

        public async Task<List<MenuDTO>> lista(int idUsuario)
        {
            IQueryable<Usuario> tbUsuario = await _usuarioRepository.Consultar(u => u.IdUsuario == idUsuario);
            IQueryable<MenuRol> tbMenuRol = await _menuRolRepository.Consultar();
            IQueryable<Menu> tbMenu = await _menuRepository.Consultar();
            try {
                IQueryable<Menu> tbResultado = (from u in tbUsuario
                                                join mr in tbMenuRol on u.IdRol equals mr.IdRol
                                                join m in tbMenu on mr.IdMenu equals m.IdMenu
                                                select m).AsQueryable();

                var listMenus = tbResultado.ToList();

                return _mapper.Map<List<MenuDTO>>(listMenus);
            }
            catch {
                throw;
            }
        }
    }
}
