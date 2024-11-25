﻿using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Persistence;

public class DataContext : IdentityDbContext<AppUser>
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Activity> Activities { get; set; }
    public DbSet<ActivityAttendee> ActivityAttendees { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ActivityAttendee>(x =>
        {
            x.HasKey(aa => new { aa.AppUserId, aa.ActivityId });

            x.HasOne(u => u.AppUser)
                .WithMany(a => a.Activities)
                .HasForeignKey(a => a.AppUserId);

            x.HasOne(u => u.Activity)
                .WithMany(a => a.Attendees)
                .HasForeignKey(a => a.ActivityId);

        });
    }
}