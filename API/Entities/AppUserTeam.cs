namespace API.Entities
{
  public class AppUserTeam
  {
    public int AppUserId { get; set; }
    public AppUser User { get; set; }
    public string TeamId { get; set; }
    public Team Team { get; set; }
  }
}