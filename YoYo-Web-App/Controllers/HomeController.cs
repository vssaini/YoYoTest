using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using YoYo.Model.ViewModels;
using YoYo.Service;
using YoYo_Web_App.Models;

namespace YoYo_Web_App.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDataService _dataService;

        public HomeController(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<IActionResult> Index()
        {
            var testAthletes = new List<TestAthleteViewModel>();
            try
            {
                testAthletes = await _dataService.GetTestAthletes().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
            }

            return View(testAthletes);
        }

        public IActionResult StartTimer()
        {
            return Ok();
        }

        public IActionResult Ping()
        {
            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
