using System.ComponentModel.DataAnnotations;

namespace YellowDirectory.Models;

/// <summary>
/// LoginViewModel is the Credentials model associated to the Login view.
/// </summary>
public class LoginViewModel
{
    [Required]
    [EmailAddress]
    public string? Email { get; init; }

    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; init; }

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; init; }
}