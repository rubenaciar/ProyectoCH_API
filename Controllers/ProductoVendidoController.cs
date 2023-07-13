using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProyectoFinalCoderHouse.Models;
using ProyectoFinalCoderHouse.Repository;
using System.Collections.Generic;

namespace ProyectoFinalCoderHouse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoVendidoController : ControllerBase
    {

        private readonly ILogger<ProductoVendidoController> _logger;
        private readonly ProductoVendidoHandler _productoVendidoHandler;
        public ProductoVendidoController(ProductoVendidoHandler productoVendidoHandler, ILogger<ProductoVendidoController> logger)
        {
            _logger = logger;
            _productoVendidoHandler = productoVendidoHandler;
        }
        

        [HttpGet]
        public IEnumerable<ProductoVendido> GetAllProductosVendidos()
        {
         
            return _productoVendidoHandler.TraerListaProductoVendidos();

       
        }

        /// <summary>
        /// Traer los Productos Vendidos por un Usuario ingresando su ID.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{idProducto}")]
        public IEnumerable<ProductoVendidoInfo> GetProductosPorIdProducto(int idProducto)
        {
            return _productoVendidoHandler.TraerProductosPorIdProducto(idProducto);
        }
    }
}
