using System.Collections.Generic;

namespace Domain.Entities
{
    public class FileEntity : Bases
    {
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public ICollection<MeetingFile> MeetingFiles { get; set; }
    }
}