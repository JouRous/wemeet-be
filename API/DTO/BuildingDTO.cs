using System;
namespace API.DTO
{
	public class BuildingDTO
	{
		public int Id { get; set; }
		public DateTime? CreatedAt { get; set; } = null;
		public string Address { get; set; }
		public string Name { get; set; }

	}
}