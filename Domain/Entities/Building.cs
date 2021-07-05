using System.Collections.Generic;

namespace Domain.Entities
{
    public class Building : Bases
    {
        public virtual string Name { get; set; }
        public virtual string Address { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
    }
}