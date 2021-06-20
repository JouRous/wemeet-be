using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
	public class AppUser : IdentityUser<int>
	{
		public string Avatar { get; set; }
		public string ResetPasswordToken { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public ICollection<AppUserRole> UserRoles { get; set; }
		public ICollection<AppUserTeam> AppUserTeams { get; set; }
	}
}