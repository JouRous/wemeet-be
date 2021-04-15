using System.Collections.Generic;

namespace API.DTO
{
  public class AuthDTO
  {
    public UserDTO User { get; set; }
    public string token { get; set; }
    public IEnumerable<string> Role { get; set; }
  }
}