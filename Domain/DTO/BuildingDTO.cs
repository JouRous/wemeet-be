using System;
namespace Domain.DTO
{
    public class BuildingDTO
    {
        public int Id { get; set; }
        public DateTime? CreatedAt { get; set; } = null;
        public string Address { get; set; }
        public string Name { get; set; }
        public int RoomNumber { get; set; } = 0;
    }
}