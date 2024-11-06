using System.Text.Json;
using Microsoft.AspNetCore.Identity;

namespace PagesJaunes.Models;

public class SeedData
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly string _adminEmail;
    private readonly string _adminPassword;
    private readonly List<CreateContactViewModel> _contacts;

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

            _contacts = JsonSerializer.Deserialize<List<CreateContactViewModel>>(jsonString, options);
        }
    }

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
            foreach (var contactViewModel in _contacts)
            {
                var contact = new Contact
                {
                    Name = contactViewModel.Name,
                    Email = contactViewModel.Email,
                    Phone = contactViewModel.Phone,
                    Country = contactViewModel.Country,
                    ZipCode = contactViewModel.ZipCode,
                    City = contactViewModel.City,
                    Street = contactViewModel.Street,
                    WorkingHours = ContactViewModel.ParseToList(contactViewModel.WorkingHours)
                };

                context.Contacts.Add(contact);
            }
            await context.SaveChangesAsync();
        }
    }
}