using System.Collections.Generic;

namespace Domain.DTO
{
    public class UserWithTeamUsersDTO : UserDTO
    {
        public ICollection<TeamWithUserDTO> Teams { get; set; }
    }
}