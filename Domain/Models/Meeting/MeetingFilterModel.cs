using System;

namespace Domain.Models
{
    public class MeetingFilterModel
    {
        public string Role { get; set; }
        public string Name { get; set; } = "";
        public Guid Room { get; set; } = Guid.Empty;
        public Guid Team { get; set; } = Guid.Empty;
        public string Creator { get; set; } = "";
    }
}