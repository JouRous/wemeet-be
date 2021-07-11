using System;
using Domain.Enums;

namespace Domain.DTO
{
    public class RoomDTO
    {
        public Guid Id { get; set; }
        public DateTime? CreatedAt { get; set; } = null;
        public string Name { get; set; }
        public StatusRomEnums Status { get; set; }
        public BuildingBaseDTO Building { get; set; }
        public int Capacity { get; set; }
    }
}