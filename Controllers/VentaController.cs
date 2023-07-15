using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using ProyectoFinalCoderHouse.Controllers.DTOS;
using ProyectoFinalCoderHouse.Models;
using ProyectoFinalCoderHouse.Repository;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalCoderHouse.EntityORM;
using System;

namespace ProyectoFinalCoderHouse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentaController : ControllerBase
    {
        private readonly VentaHandler _ventaHandler;
        private readonly ProductoHandler _productoHandler;
        private readonly UsuarioHandler _usuarioHandler;
        private readonly ProductoVendidoHandler _productoVendidoHandler;

        private readonly ILogger<ProductoController> _logger;

        public VentaController(VentaHandler ventaHandler, ProductoHandler productoHandler, UsuarioHandler usuarioHandler, ProductoVendidoHandler productoVendidoHandler,ILogger<ProductoController> logger)
        {
            _logger = logger;
            _ventaHandler = ventaHandler;
            _productoHandler = productoHandler;
            _usuarioHandler = usuarioHandler;
            _productoVendidoHandler = productoVendidoHandler;
        }

        /// <summary>
        /// Traer las Ventas por Usuario ingresando su ID.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{idUsuario}")]
        public IEnumerable<VentaDTO> GetVentas(long idUsuario)
        {
            return _ventaHandler.TraerVentasPorIdUsuario(idUsuario);
        }

      
        /// <summary>
        /// Realizar una Venta enviando una la Lista de Productos con el Usuario ingresando su ID.
        /// </summary>
        /// <returns></returns>
        [HttpPost("{idUsuario}")]
        public bool PostVenta([FromBody] List<PostVenta> productosvendidos, long idUsuario)
        {
            var usuario = _usuarioHandler.TraerUsuarioPorId(idUsuario);

            if (usuario.Id <= 0) // Verifico que el Id de Usuario asociado a la venta se encuentre en la BD
            {
                return false;
            }
            return _ventaHandler.CargarVenta(productosvendidos, idUsuario);
          
            

        }

        /// <summary>
        /// Eliminar una Venta existente ingresando el ID.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{idVenta}")]
        public void DeleteProducto(long idVenta)
        {
            _ventaHandler.EliminarVenta(idVenta);
        }

        /// <summary>
        /// Traer todas las Ventas.
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "TraerVentas")]
        public List<ProductoVendido> TraerVentas()
        {
            return _ventaHandler.TraerVentas();
        }
       
    }
}
