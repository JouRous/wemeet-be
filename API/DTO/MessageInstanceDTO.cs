using API.Enums;
using System;
namespace API.Interfaces
{
	public class NotificationMessageDTO
	{
		public string Id { get; set; }
		public EntityEnum EntityType { get; set; }
		public string EntityId { get; set; }
		public string EndpointDetails { get; set; }
		public string Message { get; set; }
		public bool IsRead { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}