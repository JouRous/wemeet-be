using System;

namespace Domain.DTO
{
    public class RoomBaseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public BuildingBaseDTO Building { get; set; }
    }
}