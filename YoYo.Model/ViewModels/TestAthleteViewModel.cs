namespace YoYo.Model.ViewModels
{
    public class TestAthleteViewModel
    {
        public int Id { get; set; }
        public int AthleteId { get; set; }

        public string Name { get; set; }
        public bool IsWarned { get; set; }
        public bool IsTestStopped { get; set; }
        public string TestScore { get; set; }
    }
}
