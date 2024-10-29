using System.Text.RegularExpressions;

namespace PagesJaunes.Models;

public class Contact
{
    public long Id { get; init; }

    private string _key
    {
        get
        {
            if (_key == null)
            {
                _key = Regex.Replace(_key, "[^a-z0-9]", "-");
            }
            return _key;
        }
        set { _key = value; }
    }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string ZipCode { get; set; }
    public Dictionary<DayOfWeek, Tuple<TimeSpan, TimeSpan>> WorkingHours { get; set; } = new (7);
    
    public Contact() {}

    public Contact(string name, string email, string phone, string country, string city, string street, string zipcode,
        Dictionary<DayOfWeek, Tuple<TimeSpan, TimeSpan>> workingHours)
    {
        
        Name = name;
        Email = email;
        Phone = phone;
        Country = country;
        City = city;
        Street = street;
        ZipCode = zipcode;
        WorkingHours = workingHours;
    }
}