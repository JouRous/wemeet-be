using System;

namespace Domain.Entities
{
    public class MeetingFile
    {
        public Guid FileEntityId { get; set; }
        public FileEntity FileEntity { get; set; }
        public Guid MeetingId { get; set; }
        public Meeting Meeting { get; set; }
    }
}