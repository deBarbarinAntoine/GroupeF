namespace YellowDirectory.Models;

/// <summary>
/// MigrateContactViewModel is the Contact model associated to the migrations.
/// </summary>
public class MigrateContactViewModel
{
    public string? Name { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string? Country { get; init; }
    public string? City { get; init; }
    public string? Street { get; init; }
    public string? ZipCode { get; init; }
    public List<MigrateWorkingHours> WorkingHours { get; init; } = new(7);

}