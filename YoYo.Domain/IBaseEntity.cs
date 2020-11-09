using System;

namespace YoYo.Domain
{
    public interface IBaseEntity
    {
        int Id { get; set; }
        DateTime DateCreated { get; set; }
    }
}
