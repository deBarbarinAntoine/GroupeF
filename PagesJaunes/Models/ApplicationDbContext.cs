using Microsoft.EntityFrameworkCore;

namespace PagesJaunes.Models;

public class ApplicationDbContext : DbContext
{
    public DbSet<Contact> Contacts { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}