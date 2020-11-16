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
    public class ProcessService : IProcessService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheHelper _cacheHelper;

        public ProcessService(IUnitOfWork unitOfWork, ICacheHelper cacheHelper)
        {
            _unitOfWork = unitOfWork;
            _cacheHelper = cacheHelper;
        }

        public async Task<TestStatusViewModel> GetTestStatusAsync(TestStatusFilter testStatusFilter)
        {
            var fitnessRatings = await GetFitnessRatingsAsync().ConfigureAwait(false);

            var shuttleFitnessRatings = fitnessRatings.Where(f => f.ShuttleNo == testStatusFilter.ShuttleNumber).ToList();
            if (shuttleFitnessRatings.Count == 0)
            {
                return null;
            }

            var fitnessRating = shuttleFitnessRatings.FirstOrDefault(f => f.ShuttleLevel == testStatusFilter.ShuttleLevel);
            if (fitnessRating != null)
            {
                return GetTestStatusViewModel(fitnessRating, testStatusFilter);
            }

            testStatusFilter.ShuttleNumber += 1;
            testStatusFilter.ShuttleLevel = 1;
            return await GetTestStatusAsync(testStatusFilter).ConfigureAwait(false);
        }

        private async Task<List<FitnessRating>> GetFitnessRatingsAsync()
        {
            var fitnessRatings = _cacheHelper.GetFitnessRatingsFromCache();
            if (fitnessRatings != null && fitnessRatings.Count > 0) return fitnessRatings;

            fitnessRatings = await _unitOfWork.FitnessRatings.All().ToListAsync().ConfigureAwait(false);
            _cacheHelper.SetFitnessRatingsInCache(fitnessRatings);
            return fitnessRatings;
        }

        private static TestStatusViewModel GetTestStatusViewModel(FitnessRating fitnessRating, TestStatusFilter testStatusFilter)
        {
            var fitnessRatingSeconds = fitnessRating.CommulativeTime.TotalSeconds;
            var fitnessRatingDistance = fitnessRating.AccumulatedShuttleDistance;

            var currentShuttleSecondsLeft = fitnessRatingSeconds + 1;
            var totalTimeSeconds = fitnessRatingSeconds + testStatusFilter.TotalTimeSeconds - fitnessRatingSeconds;
            var totalDistance = fitnessRatingDistance + testStatusFilter.TotalDistance - fitnessRatingDistance;

            return new TestStatusViewModel
            {
                // Play circle info
                ShuttleLevel = fitnessRating.ShuttleLevel,
                ShuttleNumber = fitnessRating.ShuttleNo,
                Speed = fitnessRating.Speed,

                // Three columns info
                CurrentShuttleSecondsLeft = currentShuttleSecondsLeft,
                TotalTimeSeconds = totalTimeSeconds,

                AccumulatedDistance = fitnessRatingDistance,
                DistanceIncrementer = fitnessRatingDistance / currentShuttleSecondsLeft,
                TotalDistance = totalDistance
            };
        }

        public async Task<bool> WarnAthlete(int athleteId)
        {
            var testAthlete = await _unitOfWork.TestAthletes.All().FirstOrDefaultAsync(t => t.AthleteId == athleteId).ConfigureAwait(false);
            if (testAthlete == null) { return false; }

            testAthlete.IsWarned = true;
            _unitOfWork.TestAthletes.Update(testAthlete);
            var rowsAffected = await _unitOfWork.SaveAsync().ConfigureAwait(false);

            return rowsAffected > 0;
        }

        public async Task<bool> StopAthlete(int athleteId)
        {
            var testAthlete = await _unitOfWork.TestAthletes.All().FirstOrDefaultAsync(t => t.AthleteId == athleteId).ConfigureAwait(false);
            if (testAthlete == null) { return false; }

            testAthlete.IsStopped = true;
            _unitOfWork.TestAthletes.Update(testAthlete);
            var rowsAffected = await _unitOfWork.SaveAsync().ConfigureAwait(false);

            return rowsAffected > 0;
        }
    }
}
