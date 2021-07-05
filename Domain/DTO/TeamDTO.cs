
using System.Collections;
using System.Collections.Generic;
using Domain.Entities;

namespace Domain.DTO
{
    public class TeamDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
        public string CreatedAt { get; set; }
        public UserDTO Leader { get; set; }
    }
}