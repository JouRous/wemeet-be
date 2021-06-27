using System.Collections.Generic;

namespace API.DTO
{
  public class UserWithTeamDTO : UserDTO
  {
    public ICollection<TeamDTO> Teams { get; set; }
  }
}