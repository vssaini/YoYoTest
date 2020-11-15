using System.Collections.Generic;
using System.Threading.Tasks;
using YoYo.Model.ViewModels;

namespace YoYo.Service
{
    public interface IDataService
    {
        /// <summary>
        /// Get list of test athletes.
        /// </summary>
        /// <returns>List of test athletes.</returns>
        Task<List<TestAthleteViewModel>> GetTestAthletesAsync();
    }
}
