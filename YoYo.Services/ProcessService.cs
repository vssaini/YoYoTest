using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using YoYo.Infrastructure;
using YoYo.Model.ViewModels;

namespace YoYo.Service
{
    public class ProcessService : IProcessService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProcessService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TestStatusViewModel> GetTestStatus()
        {
            var fitnessRatings = await _unitOfWork.FitnessRatings.All().ToListAsync().ConfigureAwait(false);
            var fitnessRating = fitnessRatings.Where(f => f.ShuttleNo == 1).OrderBy(f => f.SpeedLevel).First();

            return new TestStatusViewModel
            {
                SpeedLevel = fitnessRating.SpeedLevel,
                ShuttleNumber = fitnessRating.ShuttleNo,
                Speed = fitnessRating.Speed,
                CurrentShuttleSecondsLeft = fitnessRating.LevelTime,
                TotalDistance = 0,
                TotalTime = 0
            };
        }
    }
}
