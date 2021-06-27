using System.Collections.Generic;

namespace API.DTO
{
  public class TeamWithUserDTO : TeamDTO
  {
    public ICollection<UserDTO> Users { get; set; }
  }
}