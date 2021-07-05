using System.Collections.Generic;
using Domain.Enums;

namespace Domain.Entities
{
    public class Room : Bases
    {
        public virtual string Name { get; set; }
        public virtual int BuildingId { get; set; }
        public virtual Building Building { get; set; }
        public virtual int Capacity { get; set; }
        public virtual StatusRomEnums Status { get; set; } = StatusRomEnums.Active;
        public virtual ICollection<Meeting> Meetings { get; set; }
    }
}