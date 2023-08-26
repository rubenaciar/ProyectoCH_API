using ProyectoFinalCoderHouse.Abstractions;
using ProyectoFinalCoderHouse.Repository;
using System.Collections.Generic;

namespace ProyectoFinalCoderHouse.Application
{

    public class Application<T> : IApplication<T> where T: IEntity //Solo pueden ingresar a la capac de aplicacion objetos del tipo Entity
    {
        IRepository<T> _repository;
        public Application(IRepository<T> repository) 
        {
         _repository = repository;
        }
        public void DeleteById(long id)
        {
          _repository.DeleteById(id);
        }

        public IList<T> GetAll()
        {
            return _repository.GetAll();
        }

        public T GetById(long id)
        {
            return _repository.GetById(id);
        }

        public T UpdateInsert(T entity)
        {
            return _repository.UpdateInsert(entity);
        }
    }
}
