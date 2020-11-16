using System;

namespace YoYo.Model
{
    public class TestStatusFilter
    {
        /// <summary>
        /// Gets or sets current shuttle level.
        /// </summary>
        public int ShuttleLevel { get; set; }

        /// <summary>
        /// Gets or sets current shuttle number.
        /// </summary>
        public int ShuttleNumber { get; set; }

        /// <summary>
        /// Gets or sets total distance for test.
        /// </summary>
        public double TotalDistance { get; set; }

        /// <summary>
        /// Gets or sets total time, in seconds, spent for test.
        /// </summary>
        public double TotalTimeSeconds { get; set; }
    }
}
