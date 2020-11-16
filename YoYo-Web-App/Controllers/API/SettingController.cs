using System;
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
        public async Task<Result<TestStatusViewModel>> StartTimer([FromBody] TestStatusFilter testStatusFilter)
        {
            var result = new Result<TestStatusViewModel>();

            try
            {
                var testStatusVm = await _processService.GetTestStatusAsync(testStatusFilter).ConfigureAwait(false);
                result.Data = testStatusVm;
            }
            catch (Exception exc)
            {
                result.Data = null;
                result.ErrorMessage = exc.Message;
            }

            return result;
        }

        [HttpPost]
        [Route("GetTimerStatus")]
        public async Task<Result<TestStatusViewModel>> GetTimerStatus([FromBody] TestStatusFilter testStatusFilter)
        {
            var result = new Result<TestStatusViewModel>();

            try
            {
                var testStatusVm = await _processService.GetTestStatusAsync(testStatusFilter).ConfigureAwait(false);
                result.Data = testStatusVm;
            }
            catch (Exception exc)
            {
                result.Data = null;
                result.ErrorMessage = exc.Message;
            }

            return result;
        }

        [HttpPost]
        [Route("WarnAthlete")]
        public async Task<Result<bool>> WarnAthlete([FromBody] TestAthleteParam testAthleteParam)
        {
            var result = new Result<bool>();

            try
            {
                var warned = await _processService.WarnAthlete(testAthleteParam.AthleteId).ConfigureAwait(false);
                result.Data = warned;
            }
            catch (Exception exc)
            {
                result.Data = false;
                result.ErrorMessage = exc.Message;
            }

            return result;
        }

        [HttpPost]
        [Route("StopAthlete")]
        public async Task<Result<StopStatus>> StopAthlete([FromBody] TestAthleteParam testAthleteParam)
        {
            var result = new Result<StopStatus>();

            try
            {
                result = await _processService.StopAthlete(testAthleteParam).ConfigureAwait(false);

            }
            catch (Exception exc)
            {
                result.ErrorMessage = exc.Message;
                result.Data = new StopStatus { IsStopped = false, Score = "" };
            }

            return result;
        }

        [HttpPost]
        [Route("UpdateAthleteTestScore")]
        public async Task<Result<bool>> UpdateAthleteTestScore([FromBody] TestAthleteParam testAthleteParam)
        {
            var result = new Result<bool>();

            try
            {
                var updated = await _processService.UpdateTestScore(testAthleteParam).ConfigureAwait(false);
                result.Data = updated;
            }
            catch (Exception exc)
            {
                result.Data = false;
                result.ErrorMessage = exc.Message;
            }

            return result;
        }
        
        // TODO: Show progress bar around play button
        // TODO: Prepare video
    }
}
