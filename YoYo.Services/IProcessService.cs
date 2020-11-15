using System.Threading.Tasks;
using YoYo.Model.ViewModels;

namespace YoYo.Service
{
    public interface IProcessService
    {
        /// <summary>
        /// Get test status.
        /// </summary>
        Task<TestStatusViewModel> GetTestStatusAsync();
    }
}
