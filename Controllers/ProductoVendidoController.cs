﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProyectoFinalCoderHouse.Controllers.DTOS;
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


        /// <summary>
        /// Traer todos los Productos Vendidos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<ProductoVendido> GetAllProductosVendidos()
        {
             return _productoVendidoHandler.TraerListaProductoVendidos();
      
        }

        
        /// <summary>
        /// Traer los Productos Vendidos por un Usuario ingresando su ID.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{idUsuario}")]
        public IEnumerable<ProductoVendidoDTO> GetProductosPorIdUsuario(long idUsuario)
        {
            return _productoVendidoHandler.TraerProductosPorIdUsuario(idUsuario);
        }
    }
}
