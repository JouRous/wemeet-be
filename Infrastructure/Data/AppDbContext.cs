using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int, IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<AppUserTeam> AppUserTeams { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<ParticipantMeeting> ParticipantMeeting { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // builder.Entity<AppUser>()
            // .HasMany(ur => ur.UserRoles)
            // .WithOne(u => u.User)
            // .HasForeignKey(ur => ur.UserId)
            // .IsRequired();

            // builder.Entity<AppRole>()
            // .HasMany(ur => ur.UserRoles)
            // .WithOne(r => r.Role)
            // .HasForeignKey(ur => ur.RoleId)
            // .IsRequired();

            builder.Entity<AppUserTeam>().HasKey(src => new { src.AppUserId, src.TeamId });

            builder.Entity<AppUserTeam>()
            .HasOne<AppUser>(ut => ut.User)
            .WithMany(u => u.AppUserTeams)
            .HasForeignKey(ut => ut.AppUserId);


            builder.Entity<AppUserTeam>()
            .HasOne<Team>(ut => ut.Team)
            .WithMany(t => t.AppUserTeams)
            .HasForeignKey(ut => ut.TeamId);

            builder.Entity<ParticipantMeeting>().HasKey(src => new { src.MeetingId, src.ParticipantId });

            builder.Entity<ParticipantMeeting>()
            .HasOne<AppUser>(pm => pm.Participant)
            .WithMany(u => u.ParticipantMeetings)
            .HasForeignKey(pm => pm.ParticipantId);

            builder.Entity<ParticipantMeeting>()
            .HasOne<Meeting>(pm => pm.Meeting)
            .WithMany(m => m.ParticipantMeetings)
            .HasForeignKey(pm => pm.MeetingId);

            builder.Entity<AppUser>()
            .HasMany(u => u.LeadTeams)
            .WithOne(t => t.Leader);
        }
    }
}