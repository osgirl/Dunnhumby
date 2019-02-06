using System;

namespace Armin.Dunnhumby.Domain.Entities
{
    public class EntityBase
    {
        public EntityBase()
        {

        }

        public int Id { get; set; }

        public DateTime? LastUpdate { get; set; }
    }
}
