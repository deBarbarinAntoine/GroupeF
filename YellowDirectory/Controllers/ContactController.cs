using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YellowDirectory.Models;

namespace YellowDirectory.Controllers;

/// <summary>
/// ContactController manages all routes regarding the contacts, except the CreateContact route.
/// </summary>
public class ContactController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ContactController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    /// <summary>
    /// Index/default route
    /// </summary>
    /// <returns>the index view</returns>
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
            WorkingHours = ContactViewModel.ParseToWorkingHours(c.WorkingHours),
        }).ToList();

        var user = await _userManager.GetUserAsync(User);
        TempData["IsAuthenticated"] = user is not null;

        return View("Index", contactViewModels);
    }

    /// <summary>
    /// Search contact route
    /// </summary>
    /// <param name="searchTerm">the name to search</param>
    /// <param name="city">the city to filter the search</param>
    /// <returns>the index view with the results of the search</returns>
    public async Task<IActionResult> Search(string? searchTerm, string? city = null)
    {
        var query = _context.Contacts.AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(c => c.Name.ToLower().Contains(searchTerm.ToLower()));
        }

        if (!string.IsNullOrEmpty(city))
        {
            query = query.Where(c => c.City.ToLower().Contains(city.ToLower()));
        }

        var contacts = await query.ToListAsync();
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
            WorkingHours = ContactViewModel.ParseToWorkingHours(c.WorkingHours),
        }).ToList();

        TempData["SearchTerm"] = searchTerm;
        TempData["City"] = city;

        var user = await _userManager.GetUserAsync(User);
        TempData["IsAuthenticated"] = user is not null;

        return View("Index", contactViewModels);
    }

    /// <summary>
    /// View contact route
    /// </summary>
    /// <param name="id">the id of the contact to display</param>
    /// <returns>the contact view</returns>
    [HttpGet, Route("/Contact/View/{id}")]
    public async Task<IActionResult> View(long id)
    {
        var user = await _userManager.GetUserAsync(User);
        TempData["IsAuthenticated"] = user is not null;

        var contact = await _context.Contacts.FindAsync(id);

        if (contact != null)
        {
            return View("Contact", contact);
        }

        TempData["Alert"] = "Contact not found!";
        return Redirect($"/Contact/Index/");
    }

    /// <summary>
    /// Edit contact route
    /// </summary>
    /// <param name="id">the id of the contact to edit</param>
    /// <returns>the edit view</returns>
    [HttpGet, Route("/Contact/Edit/{id}"), Authorize]
    public async Task<IActionResult> Edit(long? id)
    {
        var user = await _userManager.GetUserAsync(User);
        TempData["IsAuthenticated"] = user is not null;

        if (id == null)
        {
            TempData["Alert"] = "Contact not found!";
            return Redirect($"/Contact/Index/");
        }

        var contact = await _context.Contacts.FindAsync(id);
        if (contact == null)
        {
            TempData["Alert"] = "Contact not found!";
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
            WorkingHours = ContactViewModel.ParseToWorkingHours(contact.WorkingHours),
        };

        viewModel.SetStaticWorkingHours();

        return View(viewModel);
    }

    /// <summary>
    /// Edit contact treatment route
    /// </summary>
    /// <param name="id">the id of the contact to edit</param>
    /// <param name="model">the updated contact</param>
    /// <returns>redirect to the contact view of the updated contact</returns>
    [HttpPost, ActionName("Edit"), Route("/Contact/Edit/{id}"), Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long id, ContactViewModel model)
    {
        var user = await _userManager.GetUserAsync(User);
        TempData["IsAuthenticated"] = user is not null;

        if (id != model.Id)
        {
            TempData["Alert"] = "Contact not found!";
            return Redirect($"/Contact/Index/");
        }

        if (ModelState.IsValid)
        {
            try
            {
                var contact = await _context.Contacts.FindAsync(id);
                if (contact == null)
                {
                    TempData["Alert"] = "Contact not found!";
                    return Redirect($"/Contact/Index/");
                }

                model.SetWorkingHours();

                contact.Name = model.Name;
                contact.Email = model.Email;
                contact.Phone = model.Phone;
                contact.Country = model.Country;
                contact.City = model.City;
                contact.Street = model.Street;
                contact.ZipCode = model.ZipCode;
                contact.WorkingHours = ContactViewModel.ParseToList(model.WorkingHours);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(model.Id))
                {
                    TempData["Alert"] = "Contact not found!";
                    return Redirect($"/Contact/Index/");
                }
                else
                {
                    TempData["Alert"] = "Oops, something went wrong, please try again!";
                    return Redirect($"/Contact/Index/");
                }
            }

            return RedirectToAction(nameof(Index));
        }

        TempData["Message"] = "Contact modified!";
        return Redirect($"Contact/View/{model.Id}");
    }

    /// <summary>
    /// Checks if a contact exists
    /// </summary>
    /// <param name="id">the id of the contact</param>
    /// <returns>True if it exists, False otherwise</returns>
    private bool ContactExists(long id)
    {
        return _context.Contacts.Any(e => e.Id == id);
    }

    /// <summary>
    /// Delete contact route
    /// </summary>
    /// <param name="id">the id of the contact to delete</param>
    /// <returns>the contact view with a form to confirm the deletion</returns>
    [HttpGet, Route("Contact/Delete/{id}"), Authorize]
    public async Task<IActionResult> Delete(long id)
    {
        var user = await _userManager.GetUserAsync(User);
        TempData["IsAuthenticated"] = user is not null;

        Console.WriteLine($"Deleting contact {id}");
        var contact = await _context.Contacts.FindAsync(id);
        if (contact == null)
        {
            TempData["Alert"] = "Contact not found!";
            return Redirect($"/Contact/Index/");
        }

        TempData["Alert"] = "Are you sure you want to delete this contact?";
        return View("Contact", contact);
    }

    /// <summary>
    /// Delete contact treatment route
    /// </summary>
    /// <param name="id">the id of the contact to delete</param>
    /// <returns>redirect to the index view</returns>
    [HttpPost, ActionName("Delete"), Route("Contact/Delete/{id}"), Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long id)
    {
        var user = await _userManager.GetUserAsync(User);
        TempData["IsAuthenticated"] = user is not null;

        var contact = await _context.Contacts.FindAsync(id);
        if (contact == null)
        {
            TempData["Alert"] = "Contact not found!";
            return Redirect($"/Contact/Index/");
        }

        _context.Contacts.Remove(contact);
        await _context.SaveChangesAsync();

        TempData["Message"] = "Contact deleted!";
        return Redirect($"/Contact/Index/");
    }
}