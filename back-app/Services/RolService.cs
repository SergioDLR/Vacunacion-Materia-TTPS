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
            Rol rol = _context.Rol
                .Where(rol => rol.Descripcion == descripcionRol).FirstOrDefault();

            if (rol.Id == usuario.IdRol)
               return true;

            return false;
        }

        public static Rol GetRol(VacunasContext _context, int idRol)
        {
            return _context.Rol
                .Where(r => r.Id == idRol).FirstOrDefault();
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

        public static List<List<string>> VerificarRol(VacunasContext _context, List<string> errores, int idRol)
        {
            List<List<string>> erroresConcatDescripciones = new List<List<string>>();
            List<string> descripciones = new List<string>();
            Rol rolExistente = GetRol(_context, idRol);

            if (rolExistente == null)
            {
                errores.Add(string.Format("El rol con identificador {0} no está registrado en el sistema", idRol));
                descripciones.Add(null);
            }
            else
                descripciones.Add(rolExistente.Descripcion);

            erroresConcatDescripciones.Add(errores);
            erroresConcatDescripciones.Add(descripciones);

            return erroresConcatDescripciones;
        }
    }
}
