using Microsoft.AspNetCore.Http;

namespace API.Models
{
  public class RegisterModel
  {
    public string Email { get; set; }
    public string Password { get; set; }
    public IFormFile AvatarFile { get; set; }
  }
}