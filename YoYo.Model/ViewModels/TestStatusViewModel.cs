﻿using System;

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
        public double CurrentShuttleSecondsLeft { get; set; }

        /// <summary>
        /// Gets or sets total time of YoYo test.
        /// </summary>
        public int TotalTime { get; set; }

        /// <summary>
        /// Gets or sets total distance of YoYo test.
        /// </summary>
        public int TotalDistance { get; set; }

        /// <summary>
        /// Gets or sets next level start time.
        /// </summary>
        public TimeSpan NextLevelStartTime { get; set; }

        /// <summary>
        /// Gets or sets the distance incrementer.
        /// </summary>
        public double DistanceIncrementer { get; set; }

        /// <summary>
        /// Gets or sets the distance athlete has run for current shuttle.
        /// </summary>
        public int CurrentShuttleDistance { get; set; }
    }
}
