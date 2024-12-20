namespace YellowDirectory.Models;

/// <summary>
/// CreateUserViewModel is the User model associated to the CreateUser View.
/// </summary>
public class CreateUserViewModel
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}