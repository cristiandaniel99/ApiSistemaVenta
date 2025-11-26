using SistemaVenta.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.BLL.Services.Contract
{
    public interface IUsuarioService
    {


        Task<List<UsuarioDTO>> Lista();
        Task<SessionDTO> ValidarCredenciales (string correo, string clave);

        Task<UsuarioDTO> CrearUsuario (UsuarioDTO modelo);

        Task<bool> Editar(UsuarioDTO modelo);

        Task<bool> Eliminar(int idUsuario);
    }
}
