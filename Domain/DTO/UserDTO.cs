using System;
using System.Collections.Generic;
using Domain.Entities;

namespace Domain.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string Nickname { get; set; }
        public string Position { get; set; }
        public string Role { get; set; }
        public bool isFirstLogin { get; set; }
        public bool isActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
