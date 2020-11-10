using System;

namespace YoYo.Domain.Entities
{
    public class FitnessRating : IBaseEntity
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }

        public int AccumulatedShuttleDistance { get; set; }
        public int SpeedLevel { get; set; }
        public int ShuttleNo { get; set; }
        public decimal Speed { get; set; }
        public decimal LevelTime { get; set; }
        public string CommulativeTime { get; set; }
        public string StartTime { get; set; }
        public decimal ApproxVo2Max { get; set; }
    }
}
