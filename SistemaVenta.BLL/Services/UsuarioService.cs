using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.BLL.Services.Contract;
using SistemaVenta.DAL.Repositories.Contract;
using SistemaVenta.DTO;
using SistemaVenta.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVenta.BLL.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IGenericRepository<Usuario> _usuarioRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<Usuario> _passwordHasher;

        public UsuarioService(IGenericRepository<Usuario> usuarioRepository, IMapper mapper, IPasswordHasher<Usuario> passwordHasher)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
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
                // Buscar por correo (no se puede comparar la contraseña en claro cuando está hasheada)
                var queryUsuario = await _usuarioRepository.Consultar(usuario => usuario.Correo == correo);

                var usuarioEncontrado = queryUsuario.Include(rol => rol.IdRolNavigation).FirstOrDefault();

                if (usuarioEncontrado == null)
                    throw new TaskCanceledException("Usuario no existe");

                var verification = _passwordHasher.VerifyHashedPassword(usuarioEncontrado, usuarioEncontrado.Clave, clave);
                if (verification == PasswordVerificationResult.Failed)
                    throw new TaskCanceledException("Credenciales inválidas");

                return _mapper.Map<SessionDTO>(usuarioEncontrado);
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
                if (string.IsNullOrWhiteSpace(modelo.Clave))
                    throw new TaskCanceledException("La contraseña es obligatoria");

                // Evitar mapear directamente la contraseña desde DTO sin hashear.
                var usuarioEntidad = _mapper.Map<Usuario>(modelo);

                // Comprueba existencia por correo
                var existente = await _usuarioRepository.Consultar(u => u.Correo == usuarioEntidad.Correo);
                if (existente.Any())
                    throw new TaskCanceledException("Correo ya registrado");

                // Hashear la contraseña antes de persistir
                usuarioEntidad.Clave = _passwordHasher.HashPassword(usuarioEntidad, modelo.Clave);

                var usuarioCreado = await _usuarioRepository.Crear(usuarioEntidad);

                if (usuarioCreado.IdUsuario == 0)
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

                // Solo sobrescribir la contraseña si se proporciona una nueva (evitar sobrescribir con null/empty)
                if (!string.IsNullOrWhiteSpace(modelo.Clave))
                {
                    usuarioEncontrado.Clave = _passwordHasher.HashPassword(usuarioEncontrado, modelo.Clave);
                }

                usuarioEncontrado.EsActivo = usuarioModelo.EsActivo;

                bool respuesta = await _usuarioRepository.Editar(usuarioEncontrado);

                if (!respuesta) throw new TaskCanceledException("No se pudo editar el usuario");

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

                if (!respuesta) throw new TaskCanceledException("No se pudo eliminar el usuario");

                return respuesta;
            }
            catch
            {
                throw;
            }
        }
    }
}
