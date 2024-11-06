using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagesJaunes.Models;

namespace PagesJaunes.Controllers;

[Authorize]
public class CreateContactController : Controller
{
    private readonly ApplicationDbContext _context;

    public CreateContactController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View("Create");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateContactViewModel model)
    {
        if (ModelState.IsValid)
        {
            var contact = new Contact
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Country = model.Country,
                City = model.City,
                Street = model.Street,
                ZipCode = model.ZipCode,
                WorkingHours = ContactViewModel.ParseToList(model.WorkingHours),
            };

            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Contact");
        }

        return View(model);
    }
}