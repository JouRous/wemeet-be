using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
  public class AppDbContext : IdentityDbContext<AppUser, AppRole, int, IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
  {
    public AppDbContext(DbContextOptions options) : base(options)
    { }

    public DbSet<Team> Teams { get; set; }
    public DbSet<AppUserTeam> AppUserTeams { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder.Entity<AppUser>()
      .HasMany(ur => ur.UserRoles)
      .WithOne(u => u.User)
      .HasForeignKey(ur => ur.UserId)
      .IsRequired();

      builder.Entity<AppRole>()
      .HasMany(ur => ur.UserRoles)
      .WithOne(r => r.Role)
      .HasForeignKey(ur => ur.RoleId)
      .IsRequired();

      builder.Entity<AppUserTeam>().HasKey(src => new { src.AppUserId, src.TeamId });

      builder.Entity<AppUserTeam>()
      .HasOne<AppUser>(ut => ut.User)
      .WithMany(u => u.AppUserTeams)
      .HasForeignKey(ut => ut.AppUserId);


      builder.Entity<AppUserTeam>()
      .HasOne<Team>(ut => ut.Team)
      .WithMany(t => t.AppUserTeams)
      .HasForeignKey(ut => ut.TeamId);
    }
  }
}