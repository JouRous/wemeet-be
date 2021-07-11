using System.Collections.Generic;
using Domain.DTO;

namespace Domain.Models
{
    public class AuthModel
    {
        public UserDTO User { get; set; }
        public string token { get; set; }
        // public IEnumerable<string> Role { get; set; }
    }
}