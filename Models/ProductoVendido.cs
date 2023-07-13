using System;

namespace ProyectoFinalCoderHouse.Models
{
    //CLASE PRODUCTOVENDIDO
    public class ProductoVendido
    {
        #region Atributos
        private long _id;
        private int _stock;
        private long _idProducto;
        private long _idVenta;
        #endregion
        #region Constructores
        public ProductoVendido(long id, int stock, long idProducto, long idVenta)
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
        public long Id { get; set; } // Identificador unico del producto vendido
        public int Stock { get; set; } // Cantidad del producto que se vendio

        public long IdProducto { get; set; } // Identificador del producto que se vendio

        public long IdVenta { get; set; } // Identificador de la venta a la que pertenece el producto vendido

        public virtual Producto IdProductoNavigation { get; set; }

        public virtual Venta IdVentaNavigation { get; set; }
        #endregion
    }
}