namespace API.Entities
{
	public class Room : Bases
	{
		public string Name { get; set; }
		public string BuildingId {get; set; }
		public int Capacity {get; set;}
		
	}
}