using System;

namespace API.Entities
{
  public class Meeting : Bases
  {
    public string Name { get; set; }
    public DateTime MeetingDate { get; set; }
    public string Description { get; set; }
    public string[] Tags { get; set; }
    public int TeamId { get; set; }
    public Team Team { get; set; }
    public Building building { get; set; }
  }
}