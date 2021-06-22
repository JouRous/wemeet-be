namespace API.Entities
{
	public class Room : Bases
	{
		public virtual string Name { get; set; }
		public virtual int BuildingId { get; set; }
		public virtual int Capacity { get; set; }

	}
}