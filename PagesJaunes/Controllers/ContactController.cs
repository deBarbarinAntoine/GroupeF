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

    [HttpGet, Route("/Contact/View/{id}")]
    public async Task<IActionResult> View(int id)
    {
        var contact = await _context.Contacts.FindAsync(id);

        if (contact != null)
            return View("Contact", contact);

        ViewData["alert"] = "Contact not found!";
        return Redirect($"/Contact/Index/");
    }

    [HttpGet, Route("/Contact/Edit/{id}")]
    public async Task<IActionResult> Edit(long? id)
    {
        if (id == null)
        {
            ViewData["alert"] = "Contact not found!";
            return Redirect($"/Contact/Index/");
        }

        var contact = await _context.Contacts.FindAsync(id);
        if (contact == null)
        {
            ViewData["alert"] = "Contact not found!";
            return Redirect($"/Contact/Index/");
        }

        var viewModel = new ContactViewModel
        {
            Id = contact.Id,
            Name = contact.Name,
            Email = contact.Email,
            Phone = contact.Phone,
            Country = contact.Country,
            City = contact.City,
            Street = contact.Street,
            ZipCode = contact.ZipCode,
            WorkingHours = contact.WorkingHours,
        };

        return View(viewModel);
    }

    [HttpPost, ActionName("Edit"), Route("/Contact/Edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long id, ContactViewModel model)
    {
        if (id != model.Id)
        {
            ViewData["alert"] = "Contact not found!";
            return Redirect($"/Contact/Index/");
        }

        if (ModelState.IsValid)
        {
            try
            {
                var contact = await _context.Contacts.FindAsync(id);
                if (contact == null)
                {
                    ViewData["alert"] = "Contact not found!";
                    return Redirect($"/Contact/Index/");
                }

                contact.Name = model.Name;
                contact.Email = model.Email;
                contact.Phone = model.Phone;
                contact.Country = model.Country;
                contact.City = model.City;
                contact.Street = model.Street;
                contact.ZipCode = model.ZipCode;
                contact.WorkingHours = model.WorkingHours;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(model.Id))
                {
                    ViewData["alert"] = "Contact not found!";
                    return Redirect($"/Contact/Index/");
                }
                else
                {
                    ViewData["alert"] = "Oops, something went wrong, please try again!";
                    return Redirect($"/Contact/Index/");
                }
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["message"] = "Contact modified!";
        return Redirect($"Contact/View/{model.Id}");
    }

    private bool ContactExists(long id)
    {
        return _context.Contacts.Any(e => e.Id == id);
    }

    [HttpGet, Route("Contact/Delete/{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        Console.WriteLine($"Deleting contact {id}");
        var contact = await _context.Contacts.FindAsync(id);
        if (contact == null)
        {
            ViewData["alert"] = "Contact not found!";
            return Redirect($"/Contact/Index/");
        }

        ViewData["alert"] = "Are you sure you want to delete this contact?";

        return View("Contact", contact);
    }

    [HttpPost, ActionName("Delete"), Route("Contact/Delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long id)
    {
        var contact = await _context.Contacts.FindAsync(id);
        if (contact == null)
        {
            ViewData["alert"] = "Contact not found!";
            return Redirect($"/Contact/Index/");
        }

        _context.Contacts.Remove(contact);
        await _context.SaveChangesAsync();

        ViewData["message"] = "Contact deleted!";
        return Redirect($"/Contact/Index/");
    }
}