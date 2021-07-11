using System;
using System.Collections.Generic;

using Domain.Enums;

namespace Domain.DTO
{
    public class MeetingDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public UserDTO Creator { get; set; }
        public ICollection<UserBaseDTO> UserInMeeting { get; set; }
        public RoomBaseDTO Room { get; set; }
        public ICollection<TeamBaseDTO> Teams { get; set; }
        public ICollection<TagDTO> Tags { get; set; }
        public ICollection<FileDTO> Files { get; set; }
        public StatusMeeting Status { get; set; } = StatusMeeting.Waiting;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public MethodMeeting Method { get; set; } = MethodMeeting.Offline;
        public DateTime CreatedAt { get; set; }
    }
}