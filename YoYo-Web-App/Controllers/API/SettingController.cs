using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YoYo.Model.ViewModels;
using YoYo.Service;

namespace YoYo_Web_App.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingController : ControllerBase
    {
        private readonly IProcessService _processService;

        public SettingController(IProcessService processService)
        {
            _processService = processService;
        }

        [Route("StartTimer")]
        public async Task<ActionResult<TestStatusViewModel>> StartTimer()
        {
            var testStatusVm = await _processService.GetTestStatusAsync().ConfigureAwait(false);
            return testStatusVm;
        }

        [Route("GetTimerStatus")]
        public async Task<TestStatusViewModel> GetTimerStatus()
        {
            var testStatusVm = await _processService.GetTestStatusAsync().ConfigureAwait(false);
            return testStatusVm;
        }
    }
}
