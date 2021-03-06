﻿using ConnectUs.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ConnectUs.Infrastructure
{
    public class BaseDbContext : IdentityDbContext
    {
        public BaseDbContext(DbContextOptions<BaseDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Meetup> Meetups { get; set; }
        public DbSet<MeetupUser> MeetupsUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MeetupUser>()
                .HasKey(bc => new {bc.UserId, bc.MeetupId});

            builder.Entity<MeetupUser>()
                .HasOne(bc => bc.User)
                .WithMany(b => b.MeetupsJoined)
                .HasForeignKey(bc => bc.UserId);

            builder.Entity<MeetupUser>()
                .HasOne(bc => bc.Meetup)
                .WithMany(c => c.UsersJoined)
                .HasForeignKey(bc => bc.MeetupId);

            base.OnModelCreating(builder);
        }
    }
}