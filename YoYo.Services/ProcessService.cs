using Microsoft.EntityFrameworkCore;
using System;
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
        private readonly IDataService _dataService;

        public ProcessService(IUnitOfWork unitOfWork, IDataService dataService)
        {
            _unitOfWork = unitOfWork;
            _dataService = dataService;
        }

        public async Task<TestStatusViewModel> GetTestStatusAsync(TestStatusFilter testStatusFilter)
        {
            var fitnessRatings = await _dataService.GetFitnessRatingsAsync().ConfigureAwait(false);

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
                SpeedLevel = fitnessRating.SpeedLevel,
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

        public async Task<Result<StopStatus>> StopAthlete(TestAthleteParam testAthleteParam)
        {
            var result = new Result<StopStatus>();

            var testAthlete = await _unitOfWork.TestAthletes.All().FirstOrDefaultAsync(t => t.AthleteId == testAthleteParam.AthleteId).ConfigureAwait(false);
            if (testAthlete != null)
            {
                var speedLevel = testAthleteParam.ShuttleLevel - 1 > 0 ? testAthleteParam.SpeedLevel - 1 : testAthleteParam.SpeedLevel;
                var testScore = $"{speedLevel}-{testAthleteParam.ShuttleNumber}";

                testAthlete.TestScore = testScore;
                testAthlete.IsStopped = true;

                _unitOfWork.TestAthletes.Update(testAthlete);
                var rowsAffected = await _unitOfWork.SaveAsync().ConfigureAwait(false);

                result.Data = new StopStatus { IsStopped = rowsAffected > 0, Score = testScore };
            }
            else
            {
                result.Data = new StopStatus { IsStopped = false, Score = "" };
            }

            return result;
        }

        public async Task<bool> UpdateTestScore(TestAthleteParam testAthleteParam)
        {
            var testAthlete = await _unitOfWork.TestAthletes.All().FirstOrDefaultAsync(t => t.AthleteId == testAthleteParam.AthleteId).ConfigureAwait(false);
            if (testAthlete == null) { return false; }

            testAthlete.TestScore = testAthleteParam.TestScore;

            _unitOfWork.TestAthletes.Update(testAthlete);
            var rowsAffected = await _unitOfWork.SaveAsync().ConfigureAwait(false);

            return rowsAffected > 0;
        }
    }
}
