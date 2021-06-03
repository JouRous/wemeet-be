using System;
namespace API.Data.Models.Shared
{

	public class BaseEntity
	{
		public virtual Guid Id { get; set; }
		public virtual DateTime CreatedAt { get; set; }
		public virtual DateTime UpdatedAt { get; set; }

		public BaseEntity()
		{
			Id = Guid.NewGuid();
			CreatedAt = DateTime.Now;
			UpdatedAt = DateTime.Now;

		}

	}
}