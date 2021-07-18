using System;

namespace Domain.DTO
{
    public class UserBaseDTO
    {
        public Guid Id { get; set; }
        public string Fullname { get; set; }
        public string UnsignedName { get; set; }
        public string Nickname { get; set; }
        public string Avatar { get; set; }
    }
}