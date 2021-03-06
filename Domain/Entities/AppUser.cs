using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string Fullname { get; set; }
        public string UnsignedName { get; set; }
        public string Nickname { get; set; }
        public string Position { get; set; }
        public string Avatar { get; set; }
        public string Role { get; set; }
        public bool isFirstLogin { get; set; } = false;
        public bool isActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? DeletedAt { get; set; }
        public ICollection<Team> LeadTeams { get; set; }
        public ICollection<AppUserTeam> AppUserTeams { get; set; } = new List<AppUserTeam>();
        public ICollection<ParticipantMeeting> ParticipantMeetings { get; set; }
    }
}