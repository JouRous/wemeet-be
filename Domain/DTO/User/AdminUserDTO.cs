using System;

namespace Domain.DTO
{
    public class AdminUserDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}