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
        public int ShuttleLevel { get; set; }

        /// <summary>
        /// Gets or sets current shuttle speed level.
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
        /// Gets or sets the progress step.
        /// </summary>
        public double ProgressStep { get; set; }


        /// <summary>
        /// Gets or sets seconds left of current shuttle.
        /// </summary>
        public double CurrentShuttleSecondsLeft { get; set; }

        /// <summary>
        /// Gets or sets time starter second.
        /// </summary>
        public double TimeStarterSecond { get; set; }

        /// <summary>
        /// Gets or sets time limit second.
        /// </summary>
        public double TimeLimitSecond { get; set; }


        /// <summary>
        /// Gets or sets the distance incrementer.
        /// </summary>
        public double DistanceIncrementer { get; set; }

        /// <summary>
        /// Gets or sets the accumulated distance of current shuttle.
        /// </summary>
        public int AccumulatedDistance { get; set; }

        /// <summary>
        /// Gets or sets distance starter.
        /// </summary>
        public double DistanceStarter { get; set; }

        /// <summary>
        /// Gets or sets distance limit.
        /// </summary>
        public double DistanceLimit { get; set; }
    }
}
