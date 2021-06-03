using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using API.Data.Models.Entities;

namespace API.Data.Repository.implementations
{
	public abstract class WemeetDbContext : DbContext
	{
		public WemeetDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
		{
		}
		public WemeetDbContext() : base()
		{
		}
	}
}