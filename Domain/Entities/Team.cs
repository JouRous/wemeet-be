using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Team : Bases
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public Guid LeaderId { get; set; }
        public AppUser Leader { get; set; }
        public ICollection<AppUserTeam> AppUserTeams { get; set; } = new List<AppUserTeam>();
        public ICollection<AppUser> Users { get; set; }
        public virtual ICollection<MeetingTeam> MeetingTeams { get; set; } = new List<MeetingTeam>();
    }
}