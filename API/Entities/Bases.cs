using System;
using Microsoft.AspNetCore.Identity;
namespace API.Entities
{
	public class Bases : IdentityUser<int>
	{
		public virtual DateTime CreatedAt { get; set; } = DateTime.Now;
		public virtual DateTime UpdatedAt { get; set; } = DateTime.Now;
	}
}