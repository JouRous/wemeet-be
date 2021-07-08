using System;
namespace Domain.DTO
{
    public class BuildingDTO : BuildingBaseDTO
    {
        public DateTime? CreatedAt { get; set; } = null;
        public string Address { get; set; }
        public int RoomNumber { get; set; } = 0;
    }
}