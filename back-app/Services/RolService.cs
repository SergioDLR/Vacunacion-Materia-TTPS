using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacunacionApi.Models;

namespace VacunacionApi.Services
{
    public static class RolService
    {
        public static bool TieneRol(VacunasContext _context, Usuario usuario, string descripcionRol)
        {
            Rol rolOperadorNacional = _context.Rol
                .Where(rol => rol.Descripcion == descripcionRol).FirstOrDefault();

            if (rolOperadorNacional.Id == usuario.IdRol)
               return true;

            return false;
        }

        public static List<string> VerificarCredencialesUsuario(VacunasContext _context, string email, List<string> errores, string descripcionRol)
        {
            Usuario usuarioSolicitante = UsuarioService.GetUsuario(_context, email);
            if (usuarioSolicitante == null)
                errores.Add(string.Format("El usuario {0} no está registrado en el sistema", email));
            else
            {
                bool tieneRol = TieneRol(_context, usuarioSolicitante, descripcionRol);
                if (!tieneRol)
                    errores.Add(string.Format("El usuario {0} no tiene rol {1}", email, descripcionRol));
            }
          
            return errores;
        }
    }
}
