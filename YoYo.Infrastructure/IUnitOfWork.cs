using System;
using System.Threading.Tasks;
using YoYo.Domain.Entities;

namespace YoYo.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Athlete> Athletes { get; }

        Task<int> SaveAsync();
    }
}