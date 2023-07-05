using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProyectoFinalCoderHouse.Models;
using ProyectoFinalCoderHouse.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinalCoderHouse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoVendidoController : ControllerBase
    {

        private readonly ILogger<ProductoVendidoController> _logger;

        public ProductoVendidoController(ILogger<ProductoVendidoController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<ProductoVendido> GetAllProductosVendidos()
        {
         
            return ProductoVendidoHandler.TraerListaProductoVendidos();

       
        }
    }
}
