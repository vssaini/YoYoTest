namespace YoYo.Model.ViewModels
{
    /// <summary>
    /// Define properties that represent YoYo test progress status.
    /// </summary>
    public class TestStatusViewModel
    {
        /// <summary>
        /// Gets or sets current shuttle level.
        /// </summary>
        public int CurrentShuttleLevel { get; set; }

        /// <summary>
        /// Gets or sets speed level of current shuttle.
        /// </summary>
        public int SpeedLevel { get; set; }

        /// <summary>
        /// Gets or sets shuttle number of current shuttle.
        /// </summary>
        public int ShuttleNumber { get; set; }

        /// <summary>
        /// Gets or sets speed for current shuttle.
        /// </summary>
        public decimal Speed { get; set; }

        /// <summary>
        /// Gets or sets seconds left of current shuttle.
        /// </summary>
        public decimal CurrentShuttleSecondsLeft { get; set; }

        /// <summary>
        /// Gets or sets total time of YoYo test.
        /// </summary>
        public int TotalTime { get; set; }

        /// <summary>
        /// Gets or sets total distance of YoYo test.
        /// </summary>
        public int TotalDistance { get; set; }
    }
}
