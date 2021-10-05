using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageMicroservice.Interfaces
{
    public interface IRepositoryBase<T> where T: class
    {
        public List<T> GetAll();
        public T GetById(long id);
        public T Create(T entity);
        public void Delete(long id);
    }
}
