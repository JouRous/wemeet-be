using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
  public class AppUser : IdentityUser<int>
  {
    public string Avatar { get; set; }
    public ICollection<AppUserRole> UserRoles { get; set; }
  }
}