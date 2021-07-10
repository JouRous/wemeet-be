using System.Collections.Generic;

namespace Domain.Entities
{
    public class Tag : Bases
    {
        public string Name { get; set; }
        public ICollection<MeetingTag> MeetingTags { get; set; }
    }
}