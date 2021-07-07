using System;
using Domain.Enums;

namespace Domain.DTO
{
    public class NotificationMessageDTO
    {
        public int Id { get; set; }
        public EntityEnum EntityType { get; set; }
        public int EntityId { get; set; }
        public string EndpointDetails { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}