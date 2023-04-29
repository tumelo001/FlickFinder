using System.Linq.Expressions;

namespace FlickFinder.Data
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll();
        IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression);
        T GetById(Guid id);
        void Delete(T entity);
        void Create(T entity);
        void Update(T entity);
    }
}
