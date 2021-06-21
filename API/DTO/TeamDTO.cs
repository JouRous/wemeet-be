
using System.Collections;
using System.Collections.Generic;
using API.Entities;

namespace API.DTO
{
  public class TeamDTO
  {
    public int id { get; set; }
    public string Name { get; set; }
    public string Avatar { get; set; }
    public string Description { get; set; }
    public string CreatedAt { get; set; }
    public ICollection<AppUser> Users { get; set; }
  }
}