using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using YoYo.Domain.Entities;
using YoYo.Infrastructure;

namespace YoYo_Web_App.Helpers
{
    public class DataGenerator
    {
        public static void SeedAthletes(IServiceProvider serviceProvider)
        {
            using var context = new DatabaseContext(serviceProvider.GetRequiredService<DbContextOptions<DatabaseContext>>());
            if (context.Athletes.Any())
            {
                return;
            }

            context.Athletes.AddRange(new Athlete
            {
                Id = 1,
                DateCreated = DateTime.UtcNow,
                Email = "ashton@gmail.com",
                Mobile = 52685937,
                Name = "Ashton Eaton"
            },
                new Athlete
                {
                    Id = 2,
                    DateCreated = DateTime.UtcNow,
                    Email = "bryan@gmail.com",
                    Mobile = 31370078,
                    Name = "Bryan Clay"
                },
                new Athlete
                {
                    Id = 3,
                    DateCreated = DateTime.UtcNow,
                    Email = "dean@gmail.com",
                    Mobile = 20903428,
                    Name = "Dean Karnazes"
                },
                new Athlete
                {
                    Id = 4,
                    DateCreated = DateTime.UtcNow,
                    Email = "usain@gmail.com",
                    Mobile = 61619046,
                    Name = "Usain Bolt"
                });

            context.SaveChanges();
        }
    }
}
