using Microsoft.AspNetCore.Identity;

namespace YellowDirectory.Models;

/// <summary>
/// ApplicationUser is a child of IdentityUser and corresponds to a user of the YellowDirectory app.
/// </summary>
public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime CreatedDate { get; init; } = DateTime.UtcNow;
}