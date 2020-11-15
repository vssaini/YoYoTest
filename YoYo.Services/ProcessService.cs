using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoYo.Domain.Entities;
using YoYo.Infrastructure;
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

        public async Task<TestStatusViewModel> GetTestStatusAsync()
        {
            var fitnessRatings = await GetFitnessRatingsAsync().ConfigureAwait(false);
            var fitnessRating = fitnessRatings.Where(f => f.ShuttleNo == 1).OrderBy(f => f.SpeedLevel).First();

            return new TestStatusViewModel
            {
                CurrentShuttleLevel = fitnessRating.CurrentShuttleLevel,
                SpeedLevel = fitnessRating.SpeedLevel,
                ShuttleNumber = fitnessRating.ShuttleNo,
                Speed = fitnessRating.Speed,
                CurrentShuttleSecondsLeft = fitnessRating.LevelTime,
                TotalDistance = 0,
                TotalTime = 0
            };
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
