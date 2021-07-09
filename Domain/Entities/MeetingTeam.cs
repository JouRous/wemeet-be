using System;

namespace Domain.Entities
{
    public class MeetingTeam
    {
        public Guid TeamId { get; set; }
        public Team Team { get; set; }
        public Guid MeetingId { get; set; }
        public Meeting Meeting { get; set; }
    }
}