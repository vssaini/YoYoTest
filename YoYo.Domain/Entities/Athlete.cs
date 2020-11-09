using System;

namespace YoYo.Domain.Entities
{
    public class Athlete : IBaseEntity
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public int Mobile { get; set; }
    }
}
