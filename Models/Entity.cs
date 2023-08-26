using ProyectoFinalCoderHouse.Abstractions;

namespace ProyectoFinalCoderHouse.Models
{
    public abstract class Entity : IEntity
    {
        public long Id { get; set; }
    }
}
