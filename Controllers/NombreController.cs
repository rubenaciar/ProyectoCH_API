using Microsoft.AspNetCore.Mvc;

namespace ProyectoFinalCoderHouse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NombreController : ControllerBase
    {
        /*Este apartado no se suele hacer en los trabajos,pero es la forma de que puedan ponerle su nombre a su App sin tocar el Front End*/
        /// <summary>
        /// Nombre de la aplicación.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Get()
        {
            return "PROYECTO API RUBEN ACIAR";
        }
    }
}
