namespace Domain.Models
{
    public class UserActionModel
    {
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string Nickname { get; set; }
        public string Position { get; set; }
        public string Role { get; set; }
        public bool is_active { get; set; }
    }
}