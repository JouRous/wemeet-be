using System;
using System.Collections.Generic;
using API.Entities;

namespace API.DTO
{
  public class UserDTO
  {
    public int Id { get; set; }
    public string Fullname { get; set; }
    public string Email { get; set; }
    public string Avatar { get; set; }
    public string Nickname { get; set; }
    public string Position { get; set; }
    public bool isFirstLogin { get; set; }
    public string isDeactivated { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<Team> Teams { get; set; }
  }
}
