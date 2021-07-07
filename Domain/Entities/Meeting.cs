using System;
using System.Collections.Generic;
using Domain.Enums;

namespace Domain.Entities
{
    public class Meeting : Bases
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual string Note { get; set; }
        public virtual AppUser Creator { get; set; }
        public virtual StatusMeeting Status { get; set; } = StatusMeeting.Waiting;
        public virtual DateTime StartTime { get; set; }
        public virtual DateTime EndTime { get; set; }
        public virtual PriorityMeeting Priority { get; set; } = PriorityMeeting.Normal;
        public virtual string Target { get; set; }
        public virtual MethodMeeting Method { get; set; } = MethodMeeting.Offline;
        public virtual Meeting ConflictWith { get; set; }
        public virtual Team Team { get; set; }
        public virtual Guid RoomId { get; set; }
        public virtual Room Room { get; set; }
        public virtual ICollection<ParticipantMeeting> ParticipantMeetings { get; set; } = new List<ParticipantMeeting>();
    }
}