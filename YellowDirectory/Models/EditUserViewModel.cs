namespace YellowDirectory.Models;

/// <summary>
/// EditUserViewModel is the User model associated to the EditUser view.
/// </summary>
public class EditUserViewModel
{
    public string? Id { get; init; }
    public string? UserName { get; init; }
    public string? Email { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
}