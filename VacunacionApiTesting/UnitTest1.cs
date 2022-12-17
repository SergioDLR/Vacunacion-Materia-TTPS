using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using System.Text.Json;
using VacunacionApi.Controllers;
using VacunacionApi.DTO;
using VacunacionApi.Models;

namespace VacunacionApiTesting
{
    public class UnitTest1 : ControllerBase
    {
        private readonly VacunasContext _context;
        private readonly UsuariosController _usuariosController;
        
        private readonly RequestUsuarioDTO _usuarioDTO;
        private readonly ResponseUsuarioDTO ResponseUsuarioDTO;
        //private readonly VacunasContext _vacunasContext;    
        private readonly ResponseListaUsuariosDTO _listaUsuariosDTO;    


        public UnitTest1() {
            
            _usuariosController = new UsuariosController(_context);

            _usuarioDTO = new RequestUsuarioDTO();
            ResponseUsuarioDTO = new ResponseUsuarioDTO();
            _listaUsuariosDTO = null;
        }

        [Fact]
        public async void Test1()
        {
            //model del request
            _usuarioDTO.EmailAdministrador = "fabi@gmail.com";
            _usuarioDTO.Email = "operadorNacionalTest@hotmail.com";
            _usuarioDTO.Password = "123456";
            _usuarioDTO.IdJurisdiccion = 1;
            _usuarioDTO.IdRol = 3;



            await _usuariosController.CrearUsuario(_usuarioDTO);

//            var result = await _usuariosController.CrearUsuario(_usuarioDTO);
            var result2 = await _usuariosController.GetAll(_usuarioDTO.EmailAdministrador, 1, 3);
            //Assert.IsType<OkObjectResult>(codigoHttp);


            //Assert.Equal("fabi@gmail.com", "fabi@gmail.com");

            //response
            //ResponseUsuarioDTO.EmailAdministrador = "fabi@gmail.com";
            //ResponseUsuarioDTO.ExistenciaErrores = false;
            //ResponseUsuarioDTO.EstadoTransaccion = "Aceptada";
            //ResponseUsuarioDTO.UsuarioDTO = new UsuarioDTO(0, _usuarioDTO.Email, _usuarioDTO.Password, _usuarioDTO.IdJurisdiccion, _usuarioDTO.IdRol, "Buenos Aires", "Operador Nacional");
            //Assert.Equal("fabi@gmail.com", ResponseUsuarioDTO.EmailAdministrador);
            

            //ResponseUsuarioDTO response = result.Value.Errores;

            //Assert.NotNull(result); 

            Assert.IsType<ResponseListaUsuariosDTO>(result2.Value);


          
            //Assert.True(result.IsCompletedSuccessfully);
            //Assert.IsType<OkObjectResult>(result);
            //Assert.False(condition: result.Value.ExistenciaErrores);
        }
    }
}