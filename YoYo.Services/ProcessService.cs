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

            var fitnessRating = shuttleFitnessRatings.FirstOrDefault(f => f.CurrentShuttleLevel == testStatusFilter.NextLevel);
            if (fitnessRating != null)
            {
                return new TestStatusViewModel
                {
                    CurrentShuttleLevel = fitnessRating.CurrentShuttleLevel,
                    SpeedLevel = fitnessRating.SpeedLevel,
                    ShuttleNumber = fitnessRating.ShuttleNo,
                    Speed = fitnessRating.Speed,
                    CurrentShuttleSecondsLeft = fitnessRating.CommulativeTime.TotalSeconds + 1,
                    NextLevelStartTime = fitnessRating.CommulativeTime,

                    // TODO: Set these values both in code and script end
                    TotalDistance = 0,
                    TotalTime = 0
                };
            }

            testStatusFilter.ShuttleNumber = testStatusFilter.ShuttleNumber + 1;
            testStatusFilter.NextLevel = 1;
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
    }
}
