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
        /// Gets or sets time starter second to use on JS end.
        /// </summary>
        public double TimeStarterSecond { get; set; }

        /// <summary>
        /// Gets or sets distance starter to use on JS end.
        /// </summary>
        public double DistanceStarter { get; set; }
    }
}
