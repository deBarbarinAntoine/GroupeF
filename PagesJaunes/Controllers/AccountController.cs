using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagesJaunes.Models;

namespace PagesJaunes.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet, Authorize]
    public IActionResult Index(ApplicationUser user)
    {
        return View(user);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            YamlDump.DumpAsYaml(result);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
        }

        return View(model);
    }

    [HttpGet, Authorize]
    public async Task<IActionResult> ListUsers()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is not null && user.Email == Environment.GetEnvironmentVariable("ADMIN_EMAIL"))
        {
            var users = await _userManager.Users.ToListAsync();
            var userViewModels = users.Select(u => new UserViewModel
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                CreatedDate = u.CreatedDate
            }).ToList();

            return View(userViewModels);
        }

        ViewData["Alert"] = "You're not allowed to manage users!";
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Create(CreateUserViewModel model)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is not null && user.Email == Environment.GetEnvironmentVariable("ADMIN_EMAIL"))
        {
            return View(model);
        }

        ViewData["Alert"] = "You're not allowed to create a new user!";
        return RedirectToAction("Index");
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserViewModel model)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser is not null && currentUser.Email == Environment.GetEnvironmentVariable("ADMIN_EMAIL"))
        {
            if (ModelState.IsValid)
            {
                if (model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
                    return View("Create", model);

                }

                var user = new ApplicationUser
                {
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                };

                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    ViewData["Message"] = "User created successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User creation failed.");
                }
            }

            return View("Create", model);
        }

        ViewData["Alert"] = "You're not allowed to create a new user!";
        return RedirectToAction("Index");
    }

    [HttpGet, Authorize]
    public async Task<IActionResult> Delete(string id)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser is not null && currentUser.Email == Environment.GetEnvironmentVariable("ADMIN_EMAIL"))
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                View(user);
            }
            ViewData["Alert"] = $"User {id} does not exist!";
            return RedirectToAction("ListUsers");
        }

        ViewData["Alert"] = "You're not allowed to delete a user!";
        return RedirectToAction("Index");
    }

    [HttpPost, Authorize]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser is not null && currentUser.Email == Environment.GetEnvironmentVariable("ADMIN_EMAIL"))
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    ViewData["Message"] = $"User {user.FirstName} {user.LastName} was successfully deleted!";
                    return RedirectToAction("ListUsers");
                }
                else
                {
                    Console.WriteLine($"Error deleting user: {result.Errors}");
                    ViewData["Alert"] = $"An error occurred while attempting to delete user {user.FirstName} {user.LastName}!";
                    return RedirectToAction("ListUsers");
                }
            }
            ViewData["Alert"] = $"User {user.FirstName} {user.LastName} does not exist!";
            return RedirectToAction("ListUsers");
        }

        ViewData["Alert"] = "You're not allowed to delete a user!";
        return RedirectToAction("Index");
    }

    [HttpGet, Authorize]
    public async Task<IActionResult> EditUser(string id)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser is not null && currentUser.Email == Environment.GetEnvironmentVariable("ADMIN_EMAIL"))
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewData["Alert"] = $"User {id} does not exist!";
                return RedirectToAction("ListUsers");
            }

            var viewModel = new EditUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return View(viewModel);
        }

        ViewData["Alert"] = "You're not allowed to edit a user!";
        return RedirectToAction("Index");
    }

    [HttpPost, Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUser(EditUserViewModel model)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser is not null && currentUser.Email == Environment.GetEnvironmentVariable("ADMIN_EMAIL"))
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        ViewData["Message"] = $"User {user.FirstName} {user.LastName} updated successfully!";
                        return RedirectToAction("ListUsers");
                    }
                    else
                    {
                        Console.WriteLine($"Error updating user {user.Id}: {result.Errors}");
                        ViewData["Alert"] = $"An error occurred while attempting to update user {user.FirstName} {user.LastName}!";
                        return RedirectToAction("ListUsers");
                    }
                }
            }

            return View(model);
        }

        ViewData["Alert"] = "You're not allowed to edit a user!";
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        ViewData["Message"] = "Logout Successful!";
        return RedirectToAction("Index", "Home");
    }
}