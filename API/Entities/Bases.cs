using System;
namespace API.Entities
{
	public class Bases
	{
		public virtual string Id { get; set; }
		public virtual DateTime CreatedAt { get; set; } = DateTime.Now;
		public virtual DateTime UpdatedAt { get; set; } = DateTime.Now;
	}
}