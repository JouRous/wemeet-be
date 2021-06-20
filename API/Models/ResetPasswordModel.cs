namespace API.Models
{
  public class ResetPasswordModel
  {
    public string password { get; set; }
    public string resetPasswordToken { get; set; }
  }
}