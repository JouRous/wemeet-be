
using System;
using System.Collections;
using System.Collections.Generic;
using Domain.Entities;

namespace Domain.DTO
{
    public class TeamDTO : TeamBaseDTO
    {
        public string Avatar { get; set; }
        public string Description { get; set; }
        public string CreatedAt { get; set; }
        public UserDTO Leader { get; set; }
    }
}