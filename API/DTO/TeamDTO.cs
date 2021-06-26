
using System.Collections;
using System.Collections.Generic;
using API.Entities;

namespace API.DTO
{
	public class TeamDTO
	{
		public int Id { get; set; }
		public string Name { get; set; } = null;
		public string Avatar { get; set; } = null;
		public string Description { get; set; } = null;
		public string CreatedAt { get; set; } = null;
		public ICollection<AppUser> Users { get; set; } = null;
	}
}