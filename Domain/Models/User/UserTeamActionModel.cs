using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public class UserTeamActionModel
    {
        public Guid Team_Id { get; set; }
        public ICollection<int> User_Ids { get; set; }
    }
}