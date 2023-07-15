namespace ProyectoFinalCoderHouse.Controllers.DTOS;

public partial class ProductoVendidoDTO
{
    public long Id { get; set; } // Identificador unico del producto
    public string Producto { get; set; } // Descripciones del producto
    public decimal PrecioVenta { get; set; } // Precio de venta del producto
    public int Stock { get; set; } // Cantidad en stock del producto
    public string Usuario { get; set; } // Identificador del usuario que hizo la venta

    public long IdVenta { get; set; } // Identificador de la venta
}
