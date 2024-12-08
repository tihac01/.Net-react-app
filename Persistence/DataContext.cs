using Microsoft.EntityFrameworkCore;
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
    public DbSet<Photo> Photos { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<UserFollowing> UsersFollowings { get; set; }

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

        builder.Entity<Photo>(x =>
        {
            x.HasKey(p => p.PhotoId);

            x.HasOne(p => p.User)
              .WithMany(u => u.Photos)
              .HasForeignKey(p => p.UserId)
              .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Comment>(x =>
        {
            x.HasOne(c => c.Activity)
             .WithMany(a => a.Comments)
             .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<UserFollowing>(b =>
        {
            b.HasKey(k => new { k.ObserverId, k.TargetId });

            b.HasOne(uf => uf.Observer)
                .WithMany(au => au.Followings)
                .HasForeignKey(uf => uf.ObserverId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(uf => uf.Target)
                .WithMany(au => au.Followers)
                .HasForeignKey(uf => uf.TargetId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}