using ImageMicroservice.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageMicroservice.Repositories
{
    public class Repository<T> : IRepositoryBase<T> where T : class
    {
        private readonly ImageMicroserviceDbContext _dbContext;

        public Repository(ImageMicroserviceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public T Create(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();

            return entity;
        }

        public void Delete(long id)
        {
            T entity = _dbContext.Set<T>().Find(id);
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
        }

        public List<T> GetAll()
        {
            return _dbContext.Set<T>().ToList();
        }

        public T GetById(long id)
        {
            return _dbContext.Set<T>().Find(id);
        }
    }
}
