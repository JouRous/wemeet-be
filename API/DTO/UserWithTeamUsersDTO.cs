using System.Collections.Generic;

namespace API.DTO
{
  public class UserWithTeamUsersDTO : UserDTO
  {
    public ICollection<TeamWithUserDTO> Teams { get; set; }
  }
}