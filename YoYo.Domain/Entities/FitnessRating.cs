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
        public int CurrentShuttleLevel { get; set; }

        public int AccumulatedShuttleDistance { get; set; }
        public int SpeedLevel { get; set; }
        public int ShuttleNo { get; set; }
        public decimal Speed { get; set; }
        public int LevelTime { get; set; }
        public TimeSpan CommulativeTime { get; set; }
        public string StartTime { get; set; }
        public decimal ApproxVo2Max { get; set; }
    }
}
