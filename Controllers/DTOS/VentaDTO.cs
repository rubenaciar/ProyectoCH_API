namespace ProyectoFinalCoderHouse.Controllers.DTOS;

public partial class VentaDTO
{
    public long Id { get; set; } // Identificador unico de la venta
    public string Comentarios { get; set; } // Descripciones o comentarios de la venta
    public long IdUsuario { get; set; } // Identificador del usuario que realizo la venta
    public string Usuario { get; set; } // Identificador del usuario que hizo la venta

}
