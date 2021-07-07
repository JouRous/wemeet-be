using System;
using System.Collections.Generic;

using Domain.Enums;

namespace Domain.DTO
{
    public class MeetingDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null;
        public string Description { get; set; } = null;
        public string Note { get; set; } = null;
        public UserDTO Creator { get; set; } = null;
        public ICollection<UserDTO> UserInMeeting { get; set; } = null;
        public TeamDTO Team { get; set; } = null;
        public RoomDTO Room { get; set; } = null;
        public StatusMeeting Status { get; set; } = StatusMeeting.Waiting;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public PriorityMeeting Priority { get; set; } = PriorityMeeting.Normal;
        public string Target { get; set; } = null;
        public MethodMeeting Method { get; set; } = MethodMeeting.Offline;
        public MeetingDTO ConflictWith { get; set; } = null;
        public DateTime CreatedAt { get; set; }
    }
}