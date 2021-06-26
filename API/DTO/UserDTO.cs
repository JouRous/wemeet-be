using System;
using System.Collections.Generic;
using API.Entities;

namespace API.DTO
{
	public class UserDTO
	{
		public int? Id { get; set; } = null;
		public string Fullname { get; set; } = null;
		public string Email { get; set; }
		public string Avatar { get; set; } = null;
		public string Nickname { get; set; } = null;
		public string Position { get; set; } = null;
		public bool? isFirstLogin { get; set; } = null;
		public DateTime? CreatedAt { get; set; } = null;
		public ICollection<Team> Teams { get; set; } = null;
	}
}
