using System.Collections.Generic;

namespace ProyectoFinalCoderHouse.Modelos;

public partial class Producto
{
    public long Id { get; set; }

    public string Descripciones { get; set; }

    public decimal? Costo { get; set; }

    public decimal PrecioVenta { get; set; }

    public int Stock { get; set; }

    public long IdUsuario { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; }

    public virtual ICollection<ProductoVendido> ProductoVendidos { get; set; } = new List<ProductoVendido>();
}
