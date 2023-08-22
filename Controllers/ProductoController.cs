using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using ProyectoFinalCoderHouse.Models;
using ProyectoFinalCoderHouse.Repository;
using ProyectoFinalCoderHouse.EntityORM;

namespace ProyectoFinalCoderHouse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //public class ProductoController : ControllerBase
    public class ProductoController : GenericCrudController<Producto>
    {
        private readonly ILogger<ProductoController> _logger;
        private readonly ProductoHandler _productoHandler;
        public ProductoController( ProductoHandler productoHandler, ILogger<ProductoController> logger,
        SistemaGestionContext context) : base(context) // Agregar esta línea
        {
            _logger = logger;
            _productoHandler = productoHandler;
        }



        /// <summary>
        /// Traer una Lista de todos los Productos existentes.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Producto> GetAllProductos()
        {
            var productos = _productoHandler.TraerListaProductos();

            return productos;
        }
        

        /// <summary>
        /// Crear un Producto nuevo.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public void PostProducto([FromBody]Producto producto)
        {
            _productoHandler.CrearProducto(producto);
        }


        /// <summary>
        /// Modificar un Producto existente.
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public void PutProducto([FromBody]Producto producto)
        {
            _productoHandler.ModificarProducto(producto);
        }


        /// <summary>
        /// Eliminar un Producto existente ingresando el ID.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{idProducto}")]
        public void DeleteProducto(long idProducto)
        {
            _productoHandler.EliminarProducto(idProducto);
        }


        /// <summary>
        /// Traer los productos cargados por un Usuario ingresando su ID.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{idUsuario}")]
        public IEnumerable<Producto> GetProductosPorIdUsuario(long idUsuario)
        {
            var productos = _productoHandler.TraerProductosPorIdUsuario(idUsuario);

            return productos;
        }
    }
}
