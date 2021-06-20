using System.Collections.Generic;

namespace API.Models
{
  public class UserTeamActionModel
  {
    public int TeamId { get; set; }
    public ICollection<int> UserIds { get; set; }
  }
}