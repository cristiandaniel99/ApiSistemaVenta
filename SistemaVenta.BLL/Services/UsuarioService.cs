using MapsterMapper;
using Microsoft.EntityFrameworkCore;
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
    public class UsuarioService: IUsuarioService
    {


        private readonly IGenericRepository<Usuario> _usuarioRepository;

        private readonly IMapper _mapper;


        public UsuarioService(IGenericRepository<Usuario> usuarioRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }

        public async Task<List<UsuarioDTO>> Lista()
        {
            try
            {
                var queryUsuario = await _usuarioRepository.Consultar();

                var listaUsuarios = queryUsuario.Include(Rol => Rol.IdRolNavigation).ToList();

                return _mapper.Map<List<UsuarioDTO>>(listaUsuarios);

            }
            catch
            {
                throw;
            }
        }



        public async Task<SessionDTO> ValidarCredenciales(string correo, string clave)
        {
            try
            {
                var queryUsuario = await _usuarioRepository.Consultar(usuario => usuario.Correo == correo && usuario.Clave == clave);

                if(queryUsuario.FirstOrDefault()==null)
                    throw new TaskCanceledException("Usuario no existe");

                Usuario devolverUsuario =  queryUsuario.Include(rol => rol.IdRolNavigation).First();

                return _mapper.Map<SessionDTO>(devolverUsuario);
            }
            catch
            {
                  throw;

            }
        }
        public async Task<UsuarioDTO> CrearUsuario(UsuarioDTO modelo)
        {
            try
            {
                var usuarioCreado = await _usuarioRepository.Crear(_mapper.Map<Usuario>(modelo)); //interesante

                if(usuarioCreado.IdUsuario == 0) //interesante
                    throw new TaskCanceledException("No se pudo crear el usuario");


                var query = await _usuarioRepository.Consultar(u => u.IdUsuario == usuarioCreado.IdUsuario);

                usuarioCreado = query.Include(r => r.IdRolNavigation).First();

                return _mapper.Map<UsuarioDTO>(usuarioCreado);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(UsuarioDTO modelo)
        {
            try
            {
                var usuarioModelo = _mapper.Map<Usuario>(modelo);

                var usuarioEncontrado = await _usuarioRepository.Obtener(u => u.IdUsuario == usuarioModelo.IdUsuario);

                if (usuarioEncontrado == null)
                    throw new TaskCanceledException("Usuario no encontrado");

                usuarioEncontrado.NombreCompleto = usuarioModelo.NombreCompleto;
                usuarioEncontrado.Correo = usuarioModelo.Correo;
                usuarioEncontrado.IdRol = usuarioModelo.IdRol;
                usuarioEncontrado.Clave = usuarioModelo.Clave;
                usuarioEncontrado.EsActivo = usuarioModelo.EsActivo;
             
                bool respuesta = await _usuarioRepository.Editar(usuarioEncontrado);

                if(!respuesta) throw new TaskCanceledException("No se pudo editar el usuario");

                return respuesta;


            }

            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(int idUsuario)
        {
            try
            {
                var usuarioEncontrado = await _usuarioRepository.Obtener(u => u.IdUsuario == idUsuario);
                if (usuarioEncontrado == null)
                    throw new TaskCanceledException("Usuario no encontrado");

                bool respuesta = await _usuarioRepository.Eliminar(usuarioEncontrado);

                if(!respuesta) throw new TaskCanceledException("No se pudo eliminar el usuario");

                return respuesta;
                    }
            catch
            {
                throw;
            }
        }

       

        
    }
}
