using System;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
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
        public DbSet<FileEntity> FileEntities { get; set; }
        public DbSet<MeetingFile> MeetingFile { get; set; }
        public DbSet<MeetingTeam> MeetingTeam { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
            .Ignore(e => e.EmailConfirmed)
            .Ignore(e => e.PhoneNumber)
            .Ignore(e => e.PhoneNumberConfirmed);

            // User - Team

            builder.Entity<AppUserTeam>().HasKey(src => new { src.AppUserId, src.TeamId });

            builder.Entity<AppUserTeam>()
            .HasOne<AppUser>(ut => ut.User)
            .WithMany(u => u.AppUserTeams)
            .HasForeignKey(ut => ut.AppUserId);


            builder.Entity<AppUserTeam>()
            .HasOne<Team>(ut => ut.Team)
            .WithMany(t => t.AppUserTeams)
            .HasForeignKey(ut => ut.TeamId);

            builder.Entity<AppUser>()
            .HasMany(u => u.LeadTeams)
            .WithOne(t => t.Leader);

            // Meeting - User

            builder.Entity<ParticipantMeeting>().HasKey(src => new { src.MeetingId, src.ParticipantId });

            builder.Entity<ParticipantMeeting>()
            .HasOne<AppUser>(pm => pm.Participant)
            .WithMany(u => u.ParticipantMeetings)
            .HasForeignKey(pm => pm.ParticipantId);

            builder.Entity<ParticipantMeeting>()
            .HasOne<Meeting>(pm => pm.Meeting)
            .WithMany(m => m.ParticipantMeetings)
            .HasForeignKey(pm => pm.MeetingId);



            // Meeting - Tag

            builder.Entity<MeetingTag>().HasKey(src => new { src.MeetingId, src.TagId });

            builder.Entity<MeetingTag>()
            .HasOne<Meeting>(mt => mt.Meeting)
            .WithMany(m => m.MeetingTags)
            .HasForeignKey(pm => pm.MeetingId);

            builder.Entity<MeetingTag>()
            .HasOne<Tag>(pm => pm.Tag)
            .WithMany(t => t.MeetingTags)
            .HasForeignKey(pm => pm.TagId);

            // Meeting - File
            builder.Entity<MeetingFile>().HasKey(src => new { src.FileEntityId, src.MeetingId });

            builder.Entity<MeetingFile>()
            .HasOne<Meeting>(mf => mf.Meeting)
            .WithMany(m => m.MeetingFiles)
            .HasForeignKey(pm => pm.MeetingId);

            builder.Entity<MeetingFile>()
            .HasOne<FileEntity>(mf => mf.FileEntity)
            .WithMany(f => f.MeetingFiles)
            .HasForeignKey(mf => mf.FileEntityId);

            // Meeting - Team
            builder.Entity<MeetingTeam>().HasKey(src => new { src.TeamId, src.MeetingId });

            builder.Entity<MeetingTeam>()
            .HasOne<Meeting>(mt => mt.Meeting)
            .WithMany(t => t.MeetingTeams)
            .HasForeignKey(mt => mt.MeetingId);

            builder.Entity<MeetingTeam>()
            .HasOne<Team>(mt => mt.Team)
            .WithMany(t => t.MeetingTeams)
            .HasForeignKey(mt => mt.TeamId);
        }
    }
}