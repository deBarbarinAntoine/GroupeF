namespace PagesJaunes.Models;

public class MigrateContactViewModel
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string ZipCode { get; set; }
    public List<MigrateWorkingHours> WorkingHours { get; set; } = new(7);

}