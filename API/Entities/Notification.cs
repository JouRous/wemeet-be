using System;
using API.Enums;

namespace API.Entities
{
	public class Notification : Bases
	{
		public virtual EntityEnum EntityType { get; set; }
		public virtual string EntityId { get; set; }
		public virtual string EndpointDetails { get; set; }
		public virtual string Message { get; set; }
		public virtual bool IsRead { get; set; }
	}
}