using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace YellowDirectory.Models;

/// <summary>
/// ApplicationDbContext holds all database-related data (Users and Contacts)
/// </summary>
/// <param name="options">the DbContext options</param>
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Contact> Contacts { get; init; }
    public DbSet<ApplicationUser> Users { get; init; }

    public DbSet<IdentityRole> Roles { get; init; }

    public DbSet<IdentityUserRole<string>> UserRoles { get; init; }

    public DbSet<IdentityUserClaim<string>> UserClaims { get; init; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.HasKey(ur => new { ur.UserId, ur.RoleId });
        });
    }
}