using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using System.Text.Json;
using VacunacionApi.Controllers;
using VacunacionApi.DTO;
using VacunacionApi.Models;
using VacunacionApi.Testing;

namespace VacunacionApiTesting
{
    public class UnitTest1
    {

        [Fact]
        public void TestValidarEmailNuevoUsuario()
        {
            //ingreso de nuevo usuario
            var usuarioDTO = new UsuarioDTO(2, "operadorNacional3@gmail.com", "12345", 3, 24, "Nacion", "Operador Nacional");

            //Arrange
            var mailValidator = new UsuariosTest();
            var emailAddress = usuarioDTO.Email;

            //Act
            bool isValid = mailValidator.isValidMail(emailAddress);
            
            Assert.True(isValid);
        }

        [Theory]
        [InlineData("mariaAdministradora@gmail.com", true)]
        [InlineData("fernandoAdministrador@gmail.com", true)]
        [InlineData("operador@gmail.com", false)]
        [InlineData("otroEmail@gmail.com", false)]
        public void ValidarEmailAdministrador(string emailAddress, bool expected)
        {
            //Arrange
            var mailValidator = new UsuariosTest();

            //Act
            bool isValid = mailValidator.isAdminEmail(emailAddress);

            //Assert
            Assert.Equal(isValid, expected);
        }
    }
}