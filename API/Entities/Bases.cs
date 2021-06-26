using System;
using Microsoft.AspNetCore.Identity;
namespace API.Entities
{
  public class Bases
  {
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public DateTime? DeletedAt { get; set; }
  }
}