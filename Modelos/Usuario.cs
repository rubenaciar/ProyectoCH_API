using System.Collections.Generic;

namespace ProyectoFinalCoderHouse.Modelos;

public partial class Usuario
{
    public long Id { get; set; }

    public string Nombre { get; set; }

    public string Apellido { get; set; }

    public string NombreUsuario { get; set; }

    public string Contraseña { get; set; }

    public string Mail { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
