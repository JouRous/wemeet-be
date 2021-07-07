using System;

namespace Domain.Entities
{
    public class AppUserTeam
    {
        public int AppUserId { get; set; }
        public AppUser User { get; set; }
        public Guid TeamId { get; set; }
        public Team Team { get; set; }
    }
}