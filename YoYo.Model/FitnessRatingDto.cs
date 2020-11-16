using System;
using Newtonsoft.Json;

namespace YoYo.Model
{
    public class FitnessRatingDto
    {
        public int AccumulatedShuttleDistance { get; set; }
        public int SpeedLevel { get; set; }
        public int ShuttleNo { get; set; }
        public decimal Speed { get; set; }
        public decimal LevelTime { get; set; }

        [JsonConverter(typeof(TimespanConverter))]
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public TimeSpan CommulativeTime { get; set; }

        public string StartTime { get; set; }
        public decimal ApproxVo2Max { get; set; }
    }
}
