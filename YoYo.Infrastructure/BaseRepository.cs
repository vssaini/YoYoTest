using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using YoYo.Domain;

namespace YoYo.Infrastructure
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, IBaseEntity
    {
        private readonly DatabaseContext _context;

        public BaseRepository(DatabaseContext context)
        {
            _context = context;
        }

        public virtual IQueryable<T> All()
        {
            IQueryable<T> query = _context.Set<T>();
            return query;
        }

        public virtual void Delete(T entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _context.Set<T>().Attach(entityToDelete);
            }
            _context.Set<T>().Remove(entityToDelete);
        }

        public virtual void Add(T entity)
        {
            entity.DateCreated = DateTime.UtcNow;
            _context.Set<T>().Add(entity);
        }

        public virtual void AddRange(List<T> entities)
        {
            entities.ForEach(e => e.DateCreated = DateTime.UtcNow);
            _context.Set<T>().AddRange(entities);
        }

        public virtual void Update(T entityToUpdate)
        {
            _context.Set<T>().Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}
