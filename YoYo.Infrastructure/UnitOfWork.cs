using System;
using System.Threading.Tasks;
using YoYo.Domain.Entities;

namespace YoYo.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _isDisposed;
        private readonly DatabaseContext _context;

        private IBaseRepository<Athlete> _athletes;

        public IBaseRepository<Athlete> Athletes => _athletes ??= new BaseRepository<Athlete>(_context);

        public UnitOfWork(DatabaseContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed && disposing)
            {
                _context.Dispose();
            }
            _isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
