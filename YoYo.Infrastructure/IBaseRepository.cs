using System.Collections.Generic;
using System.Linq;
using YoYo.Domain;

namespace YoYo.Infrastructure
{
    public interface IBaseRepository<T> where T : class, IBaseEntity
    {
        IQueryable<T> All();
        void Add(T entity);
        void AddRange(List<T> entities);
        void Update(T entityToUpdate);
        void Delete(T entityToDelete);
    }
}
