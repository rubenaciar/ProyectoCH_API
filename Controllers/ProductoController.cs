using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using ProyectoFinalCoderHouse.Models;
using ProyectoFinalCoderHouse.Repository;

namespace ProyectoFinalCoderHouse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly ILogger<ProductoController> _logger;
        private readonly ProductoHandler _productoHandler;
        public ProductoController(ProductoHandler productoHandler, ILogger<ProductoController> logger)
        {
            _logger = logger;
            _productoHandler = productoHandler;
        }

        [HttpGet]
       
        public IEnumerable<Producto> GetAllProductos()
        {
            var productos = _productoHandler.TraerListaProductos();

            return productos;
        }


        [HttpPut]
        public void PutProducto([FromBody]Producto producto)
        {

            _productoHandler.ModificarProducto(producto);
        }

        [HttpPost]
        public void PostProducto([FromBody]Producto producto)
        {
            _productoHandler.CrearProducto(producto);
        }

        [HttpDelete("{idProducto}")]
        public void DeleteProducto([FromBody] int idProducto)
        {
            _productoHandler.EliminarProducto(idProducto);
        }
    }
}
