using Microsoft.AspNetCore.Http;

namespace API.DTO
{
  public class RegisterDTO
  {
    public string Username { get; set; }
    public string Password { get; set; }
    public IFormFile AvatarFile { get; set; }
  }
}