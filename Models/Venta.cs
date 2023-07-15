using System;
using System.Collections.Generic;

namespace ProyectoFinalCoderHouse.Models
{
    //CLASE VENTA
    public class Venta
    {
        #region Atributos
        private long _id;
        private string _comentarios;
        private long _idUsuario;
        #endregion
        #region Constructores
        // Constructor por defecto
        public Venta()
        {

            // Mensaje de creacion de la instancia de Venta
            Console.WriteLine("La instancia de Venta se ha creado satisfactoriamente.");
        }
        public Venta(int id, string comentarios, int idUsuario)
        {
            _id = id;
            _comentarios = comentarios;
            _idUsuario = idUsuario;
            // Mensaje de creacion de la instancia de Venta
            Console.WriteLine("La instancia de Venta se ha creado satisfactoriamente.");
        }
        #endregion
        #region Propiedades
        public long Id { get; set; } // Identificador unico de la venta
        public string Comentarios { get; set; } // Descripciones o comentarios de la venta
        public long IdUsuario { get; set; } // Identificador del usuario que realizo la venta

        public virtual Usuario IdUsuarioNavigation { get; set; }

        public virtual ICollection<ProductoVendido> ProductoVendidos { get; set; } = new List<ProductoVendido>();

        #endregion
    }
}
