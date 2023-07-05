using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProyectoFinalCoderHouse.Controllers.DTOS;
using ProyectoFinalCoderHouse.Models;
using ProyectoFinalCoderHouse.Repository;

namespace ProyectoFinalCoderHouse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentaController : ControllerBase
    {
        private readonly ILogger<ProductoVendido> _logger;

        public VentaController(ILogger<ProductoVendido> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "TraerVentas")]
        public List<ProductoVendido> TraerVentas()
        {
            return VentaHandler.TraerVentas();
        }

        [HttpPost(Name = "CargarVenta")]    // No se reciben argumentos desde la URL. En el cuerpo de la petición está la lista de productos junto con el IdUsuario.
                                            // El nombre de los atributos, utilizado en el JSON (Postman), debe coincidir con el nombre de los atributos definidos en PostVenta.
                                            // Como se envía en el cuerpo de la petición una lista de Productos, en el método se recibe una List<PostVenta>
        public bool CargarVenta([FromBody] List<PostVenta> listaDeProductosVendidos)
        {
            // Validación:
            //  - Necesito Id y Stock de cada producto para poder actualizar las tablas Producto y ProductoVendido.
            //  - Necesito validar que el Id de Usuario sea válido (exista en base de datos)
            Producto producto = new Producto();
            Usuario usuario = new Usuario();
            foreach (PostVenta item in listaDeProductosVendidos)
            {
                producto = ProductoHandler.TraerProducto_conId(item.Id);
                if (producto.Id <= 0) // Verifico que todos los Id de Producto recibidos sean válidos
                {
                    return false;
                }

                if (item.Stock <= 0) // Verifico que las cantidades vendidas de cada producto sean mayores a cero
                {
                    return false;
                }

                if (producto.Stock < item.Stock) // Verifico que el stock del producto sea suficiente para realizar la venta
                {
                    return false;
                }

                usuario = UsuarioHandler.TraerUsuario_conId(item.IdUsuario);
                if (usuario.Id <= 0) // Verifico que el Id de Usuario asociado a la venta se encuentre en la BD
                {
                    return false;
                }
            }

            // Cargo una nueva venta en la tabla Venta
            Venta venta = new Venta();
            int idVenta = VentaHandler.CrearVenta(venta);
            // Si la Venta se cargó con exito continuo
            if (idVenta >= 0)
            {
                // Creo una lista de ProductoVendido a partir de "listaProductosVendidos"
                List<ProductoVendido> productosVendidos = new List<ProductoVendido>();
                foreach (PostVenta item in listaDeProductosVendidos)
                {
                    ProductoVendido productoVendido = new ProductoVendido();
                    productoVendido.IdProducto = (int)item.Id;
                    productoVendido.Stock = item.Stock;
                    productoVendido.IdVenta = idVenta;
                    productosVendidos.Add(productoVendido);
                }
                // Cargo los productos vendidos en la tabla ProductoVendido
                if (ProductoVendidoHandler.CargarProductosVendidos(productosVendidos))
                {
                    // Si los productos vendidos se cargaron con exito continuo
                    bool resultado = false;

                    // Actualizo el stock de cada uno de los productos vendidos en la tabla Producto
                    foreach (ProductoVendido item in productosVendidos)
                    {
                        producto.Id = item.IdProducto;
                        producto = ProductoHandler.ConsultarStock(producto);
                        producto.Stock = producto.Stock - item.Stock;
                        resultado = ProductoHandler.ActualizarStock(producto);
                        if (resultado == false) // Si no se puede actualizar el stock de alguno de los productos rompo el bucle y devuelvo falso
                        {
                            break;
                        }
                    }
                    return resultado;
                }
                else
                {
                    return false; // Si no se pudieron cargar los productos vendidos en la tabla ProductoVendido devuelvo falso.
                }
            }
            else
            {
                return false; // Si no se pudo cargar la venta en la tabla Venta devuelvo falso.
            }
        }

    }
}
