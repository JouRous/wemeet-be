using System.Collections.Generic;

namespace Domain.DTO
{
    public class TeamWithUserDTO : TeamDTO
    {
        public ICollection<UserBaseDTO> Users { get; set; }
    }
}