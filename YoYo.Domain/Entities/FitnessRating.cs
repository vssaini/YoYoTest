using System;

namespace YoYo.Domain.Entities
{
    public class FitnessRating : IBaseEntity
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Gets or sets the current shuttle level.
        /// </summary>
        public int ShuttleLevel { get; set; }

        /// <summary>
        /// Gets or sets the distance that athlete has run.
        /// </summary>
        public int AccumulatedShuttleDistance { get; set; }

        /// <summary>
        /// Gets or sets a level indicator for how fast the athletes needs to run.
        /// </summary>
        public int SpeedLevel { get; set; }
        public int ShuttleNo { get; set; }

        /// <summary>
        /// Gets or sets how fast athletes are running in km/h.
        /// </summary>
        public decimal Speed { get; set; }

        /// <summary>
        /// Gets or sets how many seconds a shuttle takes in seconds (forth and back to cones).
        /// </summary>
        public int LevelTime { get; set; }

        /// <summary>
        /// Gets or sets sum of the time the test has taken in min:sec.
        /// </summary>
        public TimeSpan CommulativeTime { get; set; }

        /// <summary>
        /// Gets or sets the start-time for this level or shuttle.
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// Gets or sets the fitness level for this shuttle.
        /// </summary>
        public decimal ApproxVo2Max { get; set; }
    }
}
