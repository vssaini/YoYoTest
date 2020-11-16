using System.Threading.Tasks;
using YoYo.Model;
using YoYo.Model.ViewModels;

namespace YoYo.Service
{
    public interface IProcessService
    {
        /// <summary>
        /// Get test status.
        /// </summary>
        /// <param name="testStatusFilter">The filter for deciding next level and shuttle.</param>
        Task<TestStatusViewModel> GetTestStatusAsync(TestStatusFilter testStatusFilter);

        /// <summary>
        /// Warn athlete by setting "IsWarned" property.
        /// </summary>
        /// <param name="athleteId">The test athlete Id</param>
        /// <returns>True if warned successfully else false.</returns>
        Task<bool> WarnAthlete(int athleteId);

        /// <summary>
        /// Stop athlete by setting "IsStopped" property.
        /// </summary>
        /// <param name="athleteId">The instance holding information for test athlete.</param>
        /// <returns>True if stopped successfully else false.</returns>
        Task<Result<StopStatus>> StopAthlete(TestAthleteParam athleteId);

        /// <summary>
        /// Update test score of athlete.
        /// </summary>
        /// <param name="testAthleteParam">The instance holding athlete id and new test score.</param>
        /// <returns>True if updated test score successfully else false.</returns>
        Task<bool> UpdateTestScore(TestAthleteParam testAthleteParam);
    }
}
