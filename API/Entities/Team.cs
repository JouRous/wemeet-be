using System;
using System.Collections.Generic;

namespace API.Entities
{
  public class Team : Bases
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public string Avatar { get; set; }
    public int LeaderId { get; set; }
    public AppUser Leader { get; set; }
    public ICollection<AppUserTeam> AppUserTeams { get; set; }
    public ICollection<AppUser> Users { get; set; }
    public ICollection<Meeting> Meetings { get; set; }
  }
}