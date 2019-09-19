using System;

namespace Balena.IOT.Entity.Entities
{
    public class IEntity
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
        
    }
}