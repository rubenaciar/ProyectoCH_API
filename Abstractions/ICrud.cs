using System.Collections.Generic;

namespace ProyectoFinalCoderHouse.Abstractions
{
    public interface ICrud<T>
    {
        T UpdateInsert(T entity);
        IList<T> GetAll();
        T GetById(long id);

        void DeleteById(long id);
    }
}
