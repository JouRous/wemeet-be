using System;

namespace Domain.Models
{
    public class RoomModel
    {
        public string Name { get; set; }
        public Guid BuildingId { get; set; }
        public int Capacity { get; set; }
    }
}