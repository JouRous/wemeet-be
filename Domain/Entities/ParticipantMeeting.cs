namespace Domain.Entities
{
    public class ParticipantMeeting
    {
        public int ParticipantId { get; set; }
        public AppUser Participant { get; set; }
        public int MeetingId { get; set; }
        public Meeting Meeting { get; set; }
    }
}