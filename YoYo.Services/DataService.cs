using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoYo.Domain.Entities;
using YoYo.Infrastructure;
using YoYo.Model;
using YoYo.Model.ViewModels;

namespace YoYo.Service
{
    public class DataService : IDataService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheHelper _cacheHelper;

        public DataService(IUnitOfWork unitOfWork, ICacheHelper cacheHelper)
        {
            _unitOfWork = unitOfWork;
            _cacheHelper = cacheHelper;
        }

        public async Task<List<FitnessRating>> GetFitnessRatingsAsync()
        {
            var fitnessRatings = _cacheHelper.GetFitnessRatingsFromCache();
            if (fitnessRatings != null && fitnessRatings.Count > 0) return fitnessRatings;

            fitnessRatings = await _unitOfWork.FitnessRatings.All().ToListAsync().ConfigureAwait(false);
            _cacheHelper.SetFitnessRatingsInCache(fitnessRatings);
            return fitnessRatings;
        }

        public async Task<List<TestAthleteViewModel>> GetTestAthletesAsync()
        {
            var testAthletes = await _unitOfWork.TestAthletes.All().Include(t => t.Athlete).ToListAsync().ConfigureAwait(false);

            //SpeedLevel-ShuttleNumber
            var fitnessRatings = await GetFitnessRatingsAsync().ConfigureAwait(false);

            var testResults = fitnessRatings.Select(f => new TestResult
            {
                Text = $"{f.SpeedLevel}-{f.ShuttleNo}",
                Value = $"{f.SpeedLevel}-{f.ShuttleNo}"
            }).ToList();

            return testAthletes.Select(t => new TestAthleteViewModel
            {
                AthleteId = t.AthleteId,
                Name = t.Athlete.Name,
                IsStopped = t.IsStopped,
                IsWarned = t.IsWarned,
                TestScore = t.TestScore,
                TestResults = testResults
            }).ToList();
        }
    }
}
