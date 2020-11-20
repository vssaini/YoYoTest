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
                IsStopped = false,
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

            var fitnessJsonPath = $"{AppDomain.CurrentDomain.GetData("DataDirectory")}//fitnessrating_beeptest.json";
            var fitnessJsonString = File.ReadAllText(fitnessJsonPath);

            var fitnessRatingDtos = JsonConvert.DeserializeObject<List<FitnessRatingDto>>(fitnessJsonString);
            var frDtoGroups = fitnessRatingDtos.GroupBy(f => f.ShuttleNo).Select(g => g.ToList());

            var fitnessRatings = new List<FitnessRating>();
            foreach (var frDtoGroup in frDtoGroups)
            {
                fitnessRatings.AddRange(frDtoGroup.Select((f, index) => new FitnessRating
                {
                    ShuttleLevel = index + 1,
                    ShuttleNo = f.ShuttleNo,

                    AccumulatedShuttleDistance = f.AccumulatedShuttleDistance,
                    SpeedLevel = f.SpeedLevel,
                    Speed = f.Speed,
                    LevelTime = (int)f.LevelTime,
                    CommulativeTime = f.CommulativeTime,
                    StartTime = f.StartTime,
                    ApproxVo2Max = f.ApproxVo2Max
                }));
            }

            context.FitnessRatings.AddRange(fitnessRatings);
        }
    }
}
