using System;

namespace VacunacionApi.Testing
{
    public class EmailNoRecibidoException : Exception
    {
        public override string Message => "Email no recibido correctamente"; 
    }
}