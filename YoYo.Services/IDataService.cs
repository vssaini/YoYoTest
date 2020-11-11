using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YoYo.Model.ViewModels;

namespace YoYo.Service
{
    public interface IDataService
    {
       Task<List<TestAthleteViewModel>> GetTestAthletes();
    }
}
