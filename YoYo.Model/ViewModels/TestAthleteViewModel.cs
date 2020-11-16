using System.Collections.Generic;

namespace YoYo.Model.ViewModels
{
    public class TestAthleteViewModel
    {
        public int AthleteId { get; set; }

        public string Name { get; set; }
        public bool IsWarned { get; set; }
        public bool IsStopped { get; set; }
        public string TestScore { get; set; }

        public List<TestResult> TestResults { get; set; }

        public TestAthleteViewModel()
        {
            TestResults = new List<TestResult>();
        }
    }
}
