using System;
namespace API.DTO
{
	public interface RoomDTO
	{
		public string Id { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public string Name { get; set; }
		public string BuildingId { get; set; }
		public int Capacity { get; set; }
	}
}