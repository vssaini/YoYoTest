using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YoYo.Model;
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

        [HttpPost]
        [Route("StartTimer")]
        public async Task<ActionResult<TestStatusViewModel>> StartTimer([FromBody]TestStatusFilter testStatusFilter)
        {
            var testStatusVm = await _processService.GetTestStatusAsync(testStatusFilter).ConfigureAwait(false);
            return testStatusVm;
        }

        [HttpPost]
        [Route("GetTimerStatus")]
        public async Task<TestStatusViewModel> GetTimerStatus([FromBody]TestStatusFilter testStatusFilter)
        {
            var testStatusVm = await _processService.GetTestStatusAsync(testStatusFilter).ConfigureAwait(false);
            return testStatusVm;
        }
    }
}
