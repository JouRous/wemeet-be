using System;
using API.Data.Models.Shared;

namespace API.Data.Models.Entities
{
	public class RoomEntity : BaseEntity
	{
		public virtual string Name { get; set; }
		public virtual Guid BuildingId { get; set; }
		public virtual string Address { get; set; }
		public virtual int size { get; set; }
		public virtual string Status { get; set; }

	}
}