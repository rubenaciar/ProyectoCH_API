using ProyectoFinalCoderHouse.Abstractions;
using System.Collections.Generic;

namespace ProyectoFinalCoderHouse.Repository
{
    public class Repository<T> : IRepository<T> where T : IEntity
    {
        IDBContext<T> _context;
        public Repository(IDBContext<T> context) 
        {
         _context = context;
        }
        public void DeleteById(long id)
        {
            _context.DeleteById(id);
        }

        public IList<T> GetAll()
        {
            return _context.GetAll();
        }

        public T GetById(long id)
        {
            return _context.GetById(id); 
        }

        public T UpdateInsert(T entity)
        {
            return _context.UpdateInsert(entity);
        }
    }
}
