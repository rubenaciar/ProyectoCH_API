using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalCoderHouse.Models
{
    //CLASE PRODUCTOVENDIDO
    public class ProductoVendido
    {
        #region Atributos
        private int _id;
        private int _stock;
        private int _idProducto;
        private int _idVenta;
        #endregion
        #region Constructores
        public ProductoVendido(int id, int stock, int idProducto, int idVenta)
        {
            _id = id;
            _stock = stock;
            _idProducto = idProducto;
            _idVenta = idVenta;
            // Mensaje de creacion de la instancia de ProductoVendido
            Console.WriteLine("La instancia de ProductoVendido se ha creado satisfactoriamente.");
        }

        // Constructor por defecto
        public ProductoVendido()
        {
            // Mensaje de creacion de la instancia de ProductoVendido
            Console.WriteLine("La instancia de ProductoVendido se ha creado satisfactoriamente.");
        }
        #endregion
        #region Propiedades
        public int Id { get; set; } // Identificador unico del producto vendido
        public int Stock { get; set; } // Cantidad del producto que se vendio

        public int IdProducto { get; set; } // Identificador del producto que se vendio

        public int IdVenta { get; set; } // Identificador de la venta a la que pertenece el producto vendido

        #endregion
    }
}