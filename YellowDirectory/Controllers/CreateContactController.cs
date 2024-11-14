using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YellowDirectory.Models;

namespace YellowDirectory.Controllers;

/// <summary>
/// CreateContactController manages the CreateContact route.
/// </summary>
[Authorize]
public class CreateContactController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public CreateContactController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    /// <summary>
    /// Create contact route
    /// </summary>
    /// <returns>the create view</returns>
    [HttpGet]
    public async Task<IActionResult> Create()
    {

        var user = await _userManager.GetUserAsync(User);
        TempData["IsAuthenticated"] = user is not null;

        return View(CreateContactViewModel.Empty());
    }

    /// <summary>
    /// Create contact treatment route
    /// </summary>
    /// <param name="model">the contact to create</param>
    /// <returns>redirect to the Contact/Index view</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateContactViewModel model)
    {
        if (ModelState.IsValid)
        {
            model.SetWorkingHours();
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

        var user = await _userManager.GetUserAsync(User);
        TempData["IsAuthenticated"] = user is not null;

        return View(model);
    }
}