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
    }
}
