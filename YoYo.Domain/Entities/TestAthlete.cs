using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace YoYo.Domain.Entities
{
    /// <summary>
    /// Defines athlete that will be participating in YoYo test.
    /// </summary>
    public class TestAthlete : IBaseEntity
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }

        [ForeignKey("Athlete")]
        public int AthleteId { get; set; }
        public virtual Athlete Athlete { get; set; }

        public bool IsWarned { get; set; }
        public bool IsTestStopped { get; set; }
        public string TestScore { get; set; }
    }
}
