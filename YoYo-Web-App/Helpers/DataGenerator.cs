using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YoYo.Domain.Entities;
using YoYo.Infrastructure;
using YoYo.Model;

namespace YoYo_Web_App.Helpers
{
    public class DataGenerator
    {
        public static void SeedDatabase(IServiceProvider serviceProvider)
        {
            using var context = new DatabaseContext(serviceProvider.GetRequiredService<DbContextOptions<DatabaseContext>>());
            SeedAthletes(context);
            SeedTestAthletes(context);
            SeedFitnessRatings(context);
            context.SaveChanges();
        }

        private static void SeedAthletes(DatabaseContext context)
        {
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

        private static void SeedTestAthletes(DatabaseContext context)
        {
            if (context.TestAthletes.Any())
            {
                return;
            }

            var athletes = context.Athletes.ToList();
            var testAthletes = athletes.Select(a => new TestAthlete
            {
                AthleteId = a.Id,
                IsWarned = false,
                IsTestStopped = false,
                TestScore = null
            });

            context.TestAthletes.AddRange(testAthletes);
        }

        private static void SeedFitnessRatings(DatabaseContext context)
        {
            if (context.FitnessRatings.Any())
            {
                return;
            }

            var fitnessJsonPath = $"{AppDomain.CurrentDomain.GetData("DataDirectory")}\\fitnessrating_beeptest.json";
            var fitnessJsonString = File.ReadAllText(fitnessJsonPath);

            var fitnessRatingDtos = JsonConvert.DeserializeObject<List<FitnessRatingDto>>(fitnessJsonString);
            var fitnessRatings = fitnessRatingDtos.GroupBy(f => f.ShuttleNo).Select((g, index) => new FitnessRating
            {
                CurrentShuttleLevel = index + 1,
                ShuttleNo = g.Key,

                AccumulatedShuttleDistance = g.First().AccumulatedShuttleDistance,
                SpeedLevel = g.First().SpeedLevel,
                Speed = g.First().Speed,
                LevelTime = g.First().LevelTime,
                CommulativeTime = g.First().CommulativeTime,
                StartTime = g.First().StartTime,
                ApproxVo2Max = g.First().ApproxVo2Max
            });

            context.FitnessRatings.AddRange(fitnessRatings);
        }
    }
}
