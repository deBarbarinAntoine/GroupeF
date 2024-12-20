using System.Text.Json;
using Microsoft.AspNetCore.Identity;

namespace YellowDirectory.Models;

/// <summary>
/// SeedData is the model managing the migrations when launching the web server.
/// </summary>
public class SeedData
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly string _adminEmail;
    private readonly string _adminPassword;
    private readonly List<MigrateContactViewModel> _contacts;

    /// <summary>
    /// Basic constructor that takes the admin credentials from the environment variables
    /// and fetch the MigrateContactViewModels from the JSON file specified in the environment variables.
    /// </summary>
    /// <param name="userManager">the userManager of the application</param>
    public SeedData(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
        _adminEmail = Environment.GetEnvironmentVariable("ADMIN_EMAIL");
        _adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");

        var root = Directory.GetCurrentDirectory();
        var jsonDataFilePath = Path.Combine(Path.Combine(root, "Data"), Environment.GetEnvironmentVariable("JSON_DATA_FILE"));

        using (var reader = new StreamReader(jsonDataFilePath))
        {
            string jsonString = reader.ReadToEnd();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            _contacts = JsonSerializer.Deserialize<List<MigrateContactViewModel>>(jsonString, options);
        }
    }

    /// <summary>
    /// Runs the migrations if necessary.
    /// </summary>
    /// <param name="context">the context from which run the migrations.</param>
    public async Task SeedAsync(ApplicationDbContext context)
    {
        if (!context.Users.Any(u => u.Email == _adminEmail))
        {

            var user = new ApplicationUser
            {
                UserName = _adminEmail,
                Email = _adminEmail,
                EmailConfirmed = true,
                FirstName = "Admin",
                LastName = ""
            };

            var passwordHash = _userManager.PasswordHasher.HashPassword(user, _adminPassword);
            user.PasswordHash = passwordHash;

            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine(error.Description);
                }
            }
        }

        if (!context.Contacts.Any())
        {
            foreach (var migrateContact in _contacts)
            {
                var contact = new Contact
                {
                    Name = migrateContact.Name,
                    Email = migrateContact.Email,
                    Phone = migrateContact.Phone,
                    Country = migrateContact.Country,
                    ZipCode = migrateContact.ZipCode,
                    City = migrateContact.City,
                    Street = migrateContact.Street,
                    WorkingHours = ContactViewModel.ParseToList(MigrateWorkingHours.ToWorkingHours(migrateContact.WorkingHours)),
                };

                context.Contacts.Add(contact);
            }
            await context.SaveChangesAsync();
        }
    }
}