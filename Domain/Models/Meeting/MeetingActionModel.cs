
using System;
using System.Collections.Generic;
using Domain.Enums;
using Domain.DTO;

namespace Domain.Models
{
    public class MeetingActionModel
    {
        public string Name { get; set; } = null;
        public string Description { get; set; } = null;
        public string Note { get; set; } = null;
        public ICollection<UserDTO> UserInMeeting { get; set; } = null;
        public DateTime? StartTime { get; set; } = null;
        public DateTime? EndTime { get; set; } = null;
        public PriorityMeeting Priority { get; set; } = PriorityMeeting.Normal;
        public string Target { get; set; } = null;
        public MethodMeeting Method { get; set; } = MethodMeeting.Offline;
    }
}