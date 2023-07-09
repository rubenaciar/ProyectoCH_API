namespace ProyectoFinalCoderHouse.Modelos;

public partial class ProductoVendido
{
    public long Id { get; set; }

    public int Stock { get; set; }

    public long IdProducto { get; set; }

    public long IdVenta { get; set; }

    public virtual Producto IdProductoNavigation { get; set; }

    public virtual Venta IdVentaNavigation { get; set; }
}
