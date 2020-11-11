using System;
using System.Threading.Tasks;
using YoYo.Domain.Entities;

namespace YoYo.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Athlete> Athletes { get; }
        IBaseRepository<TestAthlete> TestAthletes { get; }

        Task<int> SaveAsync();
    }
}