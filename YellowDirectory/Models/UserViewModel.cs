namespace YellowDirectory.Models;

/// <summary>
/// UserViewModel is the User model associated
/// to all user-related views except the Create view.
/// </summary>
public class UserViewModel
{
    public string? Id { get; init; }
    public string? UserName { get; init; }
    public string? Email { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public DateTime CreatedDate { get; init; }
}