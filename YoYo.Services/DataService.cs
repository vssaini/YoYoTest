﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoYo.Infrastructure;
using YoYo.Model.ViewModels;

namespace YoYo.Service
{
    public class DataService : IDataService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DataService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<TestAthleteViewModel>> GetTestAthletes()
        {
            var testAthletes = await _unitOfWork.TestAthletes.All().Include(t => t.Athlete).ToListAsync().ConfigureAwait(false);

            return testAthletes.Select(t => new TestAthleteViewModel
            {
                Id = t.Id,
                AthleteId = t.AthleteId,
                Name = t.Athlete.Name,
                IsTestStopped = t.IsTestStopped,
                IsWarned = t.IsWarned,
                TestScore = t.TestScore
            }).ToList();
        }
    }
}
