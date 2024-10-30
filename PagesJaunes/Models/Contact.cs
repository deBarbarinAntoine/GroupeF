using System.Text.RegularExpressions;

namespace PagesJaunes.Models;

public class Contact
{
    public long Id { get; init; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string ZipCode { get; set; }
    public List<string> WorkingHours { get; set; }
    
    public Contact() {}

    public Contact(string name, string email, string phone, string country, string city, string street, string zipCode,
        List<string> workingHours)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));
        }
        if (!Regex.IsMatch(email, @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$"))
        {
            throw new ArgumentException("Invalid email address.", nameof(email));
        }
        if (!Regex.IsMatch(phone, @"([+])?((\d)[.-]?)?[\s]?\(?(\d{3})\)?[.-]?[\s]?(\d{3})[.-]?[\s]?(\d{4,})"))
        {
            throw new ArgumentException("Invalid phone number.", nameof(phone));
        }
        if (string.IsNullOrWhiteSpace(country))
        {
            throw new ArgumentException("Country cannot be null or empty.", nameof(country));
        }
        if (string.IsNullOrWhiteSpace(city))
        {
            throw new ArgumentException("City cannot be null or empty.", nameof(city));
        }
        if (string.IsNullOrWhiteSpace(street))
        {
            throw new ArgumentException("Street cannot be null or empty.", nameof(street));
        }
        if (string.IsNullOrWhiteSpace(zipCode))
        {
            throw new ArgumentException("Zip code cannot be null or empty.", nameof(zipCode));  
        }
        
        Name = name;
        Email = email;
        Phone = phone;
        Country = country;
        City = city;
        Street = street;
        ZipCode = zipCode;
        WorkingHours = workingHours;
    }
}