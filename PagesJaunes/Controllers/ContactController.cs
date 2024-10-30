using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagesJaunes.Models;

namespace PagesJaunes.Controllers;

public class ContactController : Controller
{
    private readonly ApplicationDbContext _context;

    public ContactController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var contacts = await _context.Contacts.ToListAsync();
        var contactViewModels = contacts.Select(c => new ContactViewModel
        {
            Id = c.Id,
            Name = c.Name,
            Email = c.Email,
            Phone = c.Phone,
            Country = c.Country,
            City = c.City,
            Street = c.Street,
            ZipCode = c.ZipCode,
            WorkingHours = c.WorkingHours,
        }).ToList();

        return View("Index", contactViewModels);
    }
}