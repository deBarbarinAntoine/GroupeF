using Microsoft.AspNetCore.Identity;

namespace YellowDirectory.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}