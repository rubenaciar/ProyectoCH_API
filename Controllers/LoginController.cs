using Microsoft.AspNetCore.Mvc;
using ProyectoFinalCoderHouse.Controllers.DTOS;
using ProyectoFinalCoderHouse.Models;
using ProyectoFinalCoderHouse.Repository;
namespace ProyectoFinalCoderHouse.Controllers;

[ApiController]
[Route("[controller]")]

public class LoginController : ControllerBase
{
   


    [HttpGet(Name = "InicioDeSesion")] // Se reciben los parámetros nombreUsuario y contraseña desde la URL. El cuerpo de la petición siempre está vacío.
    public Usuario InicioDeSesion(string nombreUsuario, string contraseña)
    {
        return UsuarioHandler.InicioDeSesion(nombreUsuario, contraseña);
    }
}
