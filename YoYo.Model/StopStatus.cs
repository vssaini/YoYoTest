namespace YoYo.Model
{
    public class StopStatus
    {
        /// <summary>
        /// Gets or sets whether athlete tracking stopped or not.
        /// </summary>
        public bool IsStopped { get; set; }

        /// <summary>
        /// Gets or sets the score to be selected in test results.
        /// </summary>
        public string Score { get; set; }
    }
}
