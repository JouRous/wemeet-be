using System;
using System.Collections.Generic;
using API.Entities;

namespace API.DTO
{
  public class UserDTO
  {
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<Team> Teams { get; set; }
  }
}