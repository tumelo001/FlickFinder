using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FlickFinder.Data
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly AppDbContext _appDbContext;

        public RepositoryBase(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async void Create(T entity)
        {
           await _appDbContext.Set<T>().AddAsync(entity);
        }

        public  void Delete(T entity)
        {
             _appDbContext.Set<T>().Remove(entity);
        }

        public IQueryable<T> FindAll()
        {
            return  _appDbContext.Set<T>();
        }

        public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression)
        {
            return _appDbContext.Set<T>().Where(expression);
        }

        public async Task<T> GetById(int id)
        {
            return await _appDbContext.Set<T>().FindAsync(id);
        }

        public void Update(T entity)
        {
            _appDbContext.Set<T>().Update(entity);
        }
    }
}
