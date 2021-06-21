using System;
namespace API.DTO
{
	public class RoomDTO
	{
		public int Id { get; set; }
		public DateTime? CreatedAt { get; set; } = null;
		public string Name { get; set; }
		public string BuildingId { get; set; }
		public int Capacity { get; set; }
	}
}