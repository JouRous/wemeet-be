using System;
namespace API.DTO
{
	public interface BuildingDTO
	{
		public string Id { get; set; }
		public DateTime? CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public string Address { get; set; }
		public string Name { get; set; }

		public int RoomCount { get; set; }

	}
}