using System.Collections.Generic;

namespace ProyectoFinalCoderHouse.Modelos;

public partial class Venta
{
    public long Id { get; set; }

    public string Comentarios { get; set; }

    public long IdUsuario { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; }

    public virtual ICollection<ProductoVendido> ProductoVendidos { get; set; } = new List<ProductoVendido>();
}
