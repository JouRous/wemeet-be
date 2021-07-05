namespace Domain.Entities
{
    public class AppUserTeam
    {
        public int AppUserId { get; set; }
        public AppUser User { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
    }
}