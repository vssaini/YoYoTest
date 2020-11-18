using Microsoft.EntityFrameworkCore;
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
                var totalSeconds = fitnessRatings.Sum(f => f.CommulativeTime.TotalSeconds);
                var shuttleSeconds = shuttleFitnessRatings.Sum(s => s.CommulativeTime.TotalSeconds);
                var progressStep = shuttleSeconds / totalSeconds;

                return GetTestStatusViewModel(fitnessRating, testStatusFilter, progressStep);
            }

            testStatusFilter.ShuttleNumber += 1;
            testStatusFilter.ShuttleLevel = 1;
            return await GetTestStatusAsync(testStatusFilter).ConfigureAwait(false);
        }

        private static TestStatusViewModel GetTestStatusViewModel(FitnessRating fitnessRating, TestStatusFilter testStatusFilter, double progressStep)
        {
            var fitnessRatingSeconds = fitnessRating.CommulativeTime.TotalSeconds;
            var fitnessRatingDistance = fitnessRating.AccumulatedShuttleDistance;
            var currentShuttleSecondsLeft = fitnessRatingSeconds + 1;

            return new TestStatusViewModel
            {
                // Play circle info
                ShuttleLevel = fitnessRating.ShuttleLevel,
                SpeedLevel = fitnessRating.SpeedLevel,
                ShuttleNumber = fitnessRating.ShuttleNo,
                Speed = fitnessRating.Speed,

                // Progress bar data
                ProgressStep = progressStep,

                // Left column
                CurrentShuttleSecondsLeft = currentShuttleSecondsLeft,

                // Middle column
                TimeStarterSecond = testStatusFilter.TimeStarterSecond,
                TimeLimitSecond = testStatusFilter.TimeStarterSecond + fitnessRating.CommulativeTime.TotalSeconds,

                // Right column
                AccumulatedDistance = fitnessRatingDistance,
                DistanceIncrementer = fitnessRatingDistance / currentShuttleSecondsLeft,
                DistanceStarter = testStatusFilter.DistanceStarter,
                DistanceLimit = testStatusFilter.DistanceStarter + fitnessRating.AccumulatedShuttleDistance
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
                var testScore = $"{testAthleteParam.SpeedLevel}-{testAthleteParam.ShuttleNumber}";
                if (testAthleteParam.ShuttleLevel - 1 > 0)
                {
                    var fitnessRatings = await _dataService.GetFitnessRatingsAsync().ConfigureAwait(false);
                    var shuttleFitnessRating = fitnessRatings.Find(s => s.ShuttleNo == testAthleteParam.ShuttleNumber && s.ShuttleLevel == testAthleteParam.ShuttleLevel - 1);

                    if (shuttleFitnessRating != null)
                    {
                        testScore = $"{shuttleFitnessRating.SpeedLevel}-{testAthleteParam.ShuttleNumber}";
                    }
                }

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
