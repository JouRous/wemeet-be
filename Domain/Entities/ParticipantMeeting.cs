using System;

namespace Domain.Entities
{
    public class ParticipantMeeting
    {
        public Guid ParticipantId { get; set; }
        public AppUser Participant { get; set; }
        public Guid MeetingId { get; set; }
        public Meeting Meeting { get; set; }
    }
}