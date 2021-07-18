using System;

namespace Domain.DTO
{
    public class MeetingBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public UserBaseDTO Creator { get; set; }
        public RoomBaseDTO Room { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}