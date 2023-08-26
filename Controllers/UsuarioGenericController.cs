using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinalCoderHouse.Application;
using ProyectoFinalCoderHouse.Models;
using ProyectoFinalCoderHouse.Repository;

namespace ProyectoFinalCoderHouse.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioGenericController : ControllerBase
    {
        IApplication<Usuario> _usuarioApp;
        public UsuarioGenericController(IApplication<Usuario> usuario) 
        { 
         _usuarioApp = usuario;
        }

        [HttpGet("{idUsuario}")]
        public IActionResult Get(long idUsuario)
        {
            return Ok(_usuarioApp.GetById(idUsuario));
        }

     

            //return usuario == null ? new Usuario() : usuario;
     
    }
}
