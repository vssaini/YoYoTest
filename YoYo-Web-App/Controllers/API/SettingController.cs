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
        public async Task<ActionResult<TestStatusViewModel>> StartTimer([FromBody] TestStatusFilter testStatusFilter)
        {
            var testStatusVm = await _processService.GetTestStatusAsync(testStatusFilter).ConfigureAwait(false);
            return testStatusVm;
        }

        [HttpPost]
        [Route("GetTimerStatus")]
        public async Task<TestStatusViewModel> GetTimerStatus([FromBody] TestStatusFilter testStatusFilter)
        {
            var testStatusVm = await _processService.GetTestStatusAsync(testStatusFilter).ConfigureAwait(false);
            return testStatusVm;
        }

        [HttpPost]
        [Route("WarnAthlete")]
        public async Task<bool> WarnAthlete([FromBody] TestAthleteParam testAthleteParam)
        {
            var isWarned = await _processService.WarnAthlete(testAthleteParam.AthleteId).ConfigureAwait(false);
            return isWarned;
        }

        [HttpPost]
        [Route("StopAthlete")]
        public async Task<bool> StopAthlete([FromBody] TestAthleteParam testAthleteParam)
        {
            var isStopped = await _processService.StopAthlete(testAthleteParam.AthleteId).ConfigureAwait(false);
            return isStopped;
        }

        // TODO: Calculate result based on last level
        // TODO: Set dropdown options and select them
        // TODO: Show progress bar around play button
        // TODO: Prepare video
    }
}
