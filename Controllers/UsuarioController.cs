using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using ProyectoFinalCoderHouse.Models;
using ProyectoFinalCoderHouse.Repository;
//using ProyectoFinalCoderHouse.Modelos;

namespace ProyectoFinalCoderHouse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioHandler _usuarioHandler;
        private readonly ILogger<Usuario> _logger;

        public UsuarioController(UsuarioHandler usuarioHandler, ILogger<Usuario> logger)
        {
            _logger = logger;
            _usuarioHandler = usuarioHandler;
        }

       
        [HttpGet]
        public List<Usuario> GetAllUsuarios()
        {
            return _usuarioHandler.TraerListaUsuarios();

            
        }
        
        
        /// <summary>
        /// Inicio de Sesión ingresando Usuario y Contaseña.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{nombreUsuario}/{contraseña}")]
        public Usuario InicioDeSesion(string nombreUsuario, string contraseña)
        {
            var usuario = _usuarioHandler.InicioDeSesion(nombreUsuario, contraseña);
            return (usuario.EsValido) ? usuario : new Usuario()
            {
                MensajeLogin = "Nombre de usuario o contraseña incorrectos",

            };
           
            
        }

        [HttpPut]
        public void PutUsuario([FromBody]Usuario usuario)
        {
            _usuarioHandler.ModificarUsuario(usuario);
        }
       
    }
}
