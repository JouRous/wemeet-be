using System;

namespace Domain.Entities
{
    public class MeetingTag
    {
        public Guid MeetingId { get; set; }
        public Meeting Meeting { get; set; }
        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
    }
}