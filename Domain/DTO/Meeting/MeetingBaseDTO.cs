using System;
using System.Collections.Generic;
using Domain.Enums;

namespace Domain.DTO
{
    public class MeetingBaseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public UserBaseDTO Creator { get; set; }
        public RoomBaseDTO Room { get; set; }
        public ICollection<TeamBaseDTO> Teams { get; set; }
        public StatusMeeting Status { get; set; } = StatusMeeting.Waiting;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}