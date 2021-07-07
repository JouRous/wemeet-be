using System.Collections.Generic;

namespace Domain.DTO
{
    public class UserWithTeamDTO : UserDTO
    {
        public ICollection<TeamDTO> Teams { get; set; }
    }
}