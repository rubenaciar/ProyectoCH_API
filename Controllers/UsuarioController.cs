using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProyectoFinalCoderHouse.Models;
using ProyectoFinalCoderHouse.Repository;

namespace ProyectoFinalCoderHouse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly ILogger<ProductoVendido> _logger;

        public UsuarioController(ILogger<ProductoVendido> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public List<Usuario> GetAllUsuarios()
        {
            var usuarios = UsuarioHandler.TraerListaUsuarios();

            return usuarios;
        }

        //[HttpGet("{nombreUsuario}/{contraseña}")]
        //public Usuario GetUsuarioByContraseña(string nombreUsuario, string contraseña)
        //{
        //    var usuario = UsuarioHandler.InicioDeSesion(nombreUsuario, contraseña);

        //    return usuario == null ? new Usuario() : usuario;
        //}

        [HttpPut]
        public void PutUsuario([FromBody]Usuario usuario)
        {
           UsuarioHandler.ModificarUsuario(usuario);
        }
       
    }
}
