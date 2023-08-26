using ProyectoFinalCoderHouse.Abstractions;
using System.Collections.Generic;
using System.Linq;
namespace ProyectoFinalCoderHouse.EntityORM
{
    public class DBContext<T> : IDBContext<T> where T : IEntity
    {
        IList<T> _data;
        public DBContext() 
        { 
         _data = new List<T>();
        }
        public void DeleteById(long id)
        {
            var e = _data.Where(u => u.Id.Equals(id)).FirstOrDefault();
            if (e != null)
            {
                _data.Remove(e);
            }
        }

        public IList<T> GetAll()
        {
            return _data;
        }

        public T GetById(long id)
        {
            return _data.Where(u => u.Id.Equals(id)).FirstOrDefault();
        }

        public T UpdateInsert(T entity)
        {
            if (entity.Id.Equals(0)) 
            {
                _data.Add(entity);
            }
            else
            {
                _data.Append(entity);
            }
            return entity;
        }
    }
}
