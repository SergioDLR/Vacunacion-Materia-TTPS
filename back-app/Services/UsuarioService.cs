using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VacunacionApi.DTO;
using VacunacionApi.Models;

namespace VacunacionApi.Services
{
    public static class UsuarioService
    {
        public static Usuario GetUsuario(VacunasContext _context, string email)
        {
            Usuario cuentaExistente = new Usuario();
            List<Usuario> listaUsuarios = _context.Usuario.ToList();

            foreach (Usuario item in listaUsuarios)
            {
                if (item.Email == email)
                {
                    cuentaExistente.Id = item.Id;
                    cuentaExistente.Email = item.Email;
                    cuentaExistente.Password = item.Password;
                    cuentaExistente.IdJurisdiccion = item.IdJurisdiccion.Value;
                    cuentaExistente.IdRol = item.IdRol;
                }
            }

            return cuentaExistente;
        }

        public static Usuario CuentaUsuarioExists(VacunasContext _context, string email)
        {
            return _context.Usuario
                .Where(user => user.Email == email).FirstOrDefault();
        }

        public static bool TieneFormatoEmail(string email)
        {
            Regex regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            Match matchEmail = regex.Match(email);

            if (matchEmail.Success)
                return true;

            return false;
        }
    }
}
