using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace VacunacionApi.Testing
{
    public class UsuariosTest
    {
        public bool isValidMail(string emailAddress)
        {
            if (string.IsNullOrEmpty(emailAddress))
            {
                throw new EmailNoRecibidoException();
            }
            Regex regex = new Regex(@"^[\w0-9._%+-]+@[\w0-9.-]+\.[\w]{2,6}$");
            return regex.IsMatch(emailAddress);
        }
        public string isExistEmail(string emailAddress)
        {
            if (string.IsNullOrEmpty(emailAddress))
            {
                throw new EmailNoRecibidoException();
            }
            List<string> listEmailsExistentes = new List<string>() { "mariaOperadorNacional@gmail.com", "fernandoAnalistaBuenosAires@gmail.com" };
            return listEmailsExistentes.Any(d => emailAddress == d) ? "Email ya existente" : "Email creado correctamente";
        }

        public bool isAdminEmail(string emailAddress)
        {
            if (string.IsNullOrEmpty(emailAddress))
            {
                throw new EmailNoRecibidoException();
            }
            List<string> listEmailsAdmins = new List<string>() { "mariaAdministradora@gmail.com", "fernandoAdministrador@gmail.com" };
            //return listEmailsExistentes.Any(d => emailAddress.Contains) ? "Email ya existente" : "Email creado correctamente";
            return listEmailsAdmins.Contains(emailAddress);
        }
    }
}