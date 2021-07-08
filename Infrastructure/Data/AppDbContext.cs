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
        public DbSet<Tag> Tags { get; set; }
        public DbSet<MeetingTag> MeetingTag { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

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

            builder.Entity<MeetingTag>().HasKey(src => new { src.MeetingId, src.TagId });

            builder.Entity<MeetingTag>()
            .HasOne<Meeting>(mt => mt.Meeting)
            .WithMany(m => m.MeetingTags)
            .HasForeignKey(pm => pm.MeetingId);

            builder.Entity<MeetingTag>()
            .HasOne<Tag>(pm => pm.Tag)
            .WithMany(t => t.MeetingTags)
            .HasForeignKey(pm => pm.TagId);
        }
    }
}