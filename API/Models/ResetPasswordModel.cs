namespace API.Models
{
  public class ResetPasswordModel
  {
    public string email { get; set; }
    public string resetPasswordToken { get; set; }
  }
}