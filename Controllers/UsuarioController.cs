using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using ProyectoFinalCoderHouse.Models;
using ProyectoFinalCoderHouse.Repository;
using static ProyectoFinalCoderHouse.Models.Usuario;
using System;
using ProyectoFinalCoderHouse.Exceptions;

//using ProyectoFinalCoderHouse.Modelos;

namespace ProyectoFinalCoderHouse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioHandler _usuarioHandler;
        private readonly ILogger<Usuario> _logger;
        private const int iLargoContraseña = 8;
        public UsuarioController(UsuarioHandler usuarioHandler, ILogger<Usuario> logger)
        {
            _logger = logger;
            _usuarioHandler = usuarioHandler;
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



        /// <summary>
        /// Traer una Lista de todos los Usuarios existentes.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<Usuario> GetAllUsuarios()
        {
            return _usuarioHandler.TraerListaUsuarios();

        }


        /// <summary>
        /// Traer datos de un Usuario ingresando su nombre de usuario.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{nombreUsuario}")]
        public Usuario GetNombreUsuario(string nombreUsuario)
        {
            var usuario = _usuarioHandler.TraerUsuarioPorNombre(nombreUsuario);

            return usuario == null ? new Usuario() : usuario;
        }


        /// <summary>
        /// Crear un Usuario nuevo.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult PostUsuario([FromBody]Usuario usuario)
        {

            try
            {
                // Se lanzarán excepciones en el constructor de Usuario si los parámetros no son válidos
                Usuario nuevoUsuario = new Usuario(usuario.Id, usuario.Nombre, usuario.Apellido, usuario.NombreUsuario, usuario.Contraseña, usuario.Mail);

                _usuarioHandler.CrearUsuario(nuevoUsuario);
                return Ok(); // Devuelve una respuesta HTTP 200 OK si el usuario se crea correctamente
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // Devuelve una respuesta HTTP 400 Bad Request con el mensaje de error
            }
            catch (ContraseñaInvalidaException ex)
            {
                return BadRequest(ex.Message); // Devuelve una respuesta HTTP 400 Bad Request con el mensaje de error de la contraseña
            }
            catch (Exception ex)
            {

                // Devuelve una respuesta HTTP 500 Internal Server Error genérica
                return StatusCode(500, "Ocurrió un error interno.");
             }
            
          
        }

        /// <summary>
        /// Modificar un Usuario existente.
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public void PutUsuario([FromBody]Usuario usuario)
        {
            _usuarioHandler.ModificarUsuario(usuario);
        }

        /// <summary>
        /// Eliminar un Usuario existente ingresando el ID.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{idUsuario}")]
        public void DeleteUsuario(int idUsuario)
        {
            _usuarioHandler.EliminarUsuario(idUsuario);
        }

      
    }
}
