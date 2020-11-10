using Microsoft.EntityFrameworkCore;
using YoYo.Domain.Entities;

namespace YoYo.Infrastructure
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }

        public virtual DbSet<Athlete> Athletes { get; set; }
        public virtual DbSet<TestAthlete> TestAthletes { get; set; }
        public virtual DbSet<FitnessRating> FitnessRatings { get; set; }
    }
}
