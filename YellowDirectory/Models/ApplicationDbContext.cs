using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace YellowDirectory.Models;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<ApplicationUser> Users { get; set; }

    public DbSet<IdentityRole> Roles { get; set; }

    public DbSet<IdentityUserRole<string>> UserRoles { get; set; }

    public DbSet<IdentityUserClaim<string>> UserClaims { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.HasKey(ur => new { ur.UserId, ur.RoleId });
        });
    }
}