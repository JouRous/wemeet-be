using System.Collections.Generic;

namespace API.Entities
{
  public class Team : Bases
  {
    public string Name { get; set; }
    public ICollection<AppUserTeam> AppUserTeams { get; set; }
  }
}