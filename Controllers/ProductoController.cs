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
    public class ProductoController : ControllerBase
    {
        private readonly ILogger<ProductoController> _logger;

        public ProductoController(ILogger<ProductoController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
       
        public IEnumerable<Producto> GetAllProductos()
        {
            var productos = ProductoHandler.TraerListaProductos();

            return productos;
        }


        [HttpPut]
        public void PutProducto([FromBody]Producto producto)
        {
            
             ProductoHandler.ModificarProducto(producto);
        }

        [HttpPost]
        public void PostProducto([FromBody]Producto producto)
        {
            ProductoHandler.CrearProducto(producto);
        }

        [HttpDelete("{idProducto}")]
        public void DeleteProducto([FromBody] int idProducto)
        {
            ProductoHandler.EliminarProducto(idProducto);
        }
    }
}
