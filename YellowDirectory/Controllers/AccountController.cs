using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YellowDirectory.Models;

namespace YellowDirectory.Controllers;

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

        TempData["IsAuthenticated"] = true;

        return View(user);
    }

    [HttpGet]
    public async Task<IActionResult> Login()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is not null)
            return RedirectToAction("Index", user);

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser is not null)
            return RedirectToAction("Index", currentUser);

        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            YamlDump.DumpAsYaml(result);

            if (result.Succeeded)
            {
                var user = await _userManager.GetUserAsync(User);
                return RedirectToAction("Index", user);
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
        TempData["IsAuthenticated"] = user is not null;
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

        TempData["Alert"] = "You're not allowed to manage users!";
        return RedirectToAction("Index", user);
    }

    [HttpGet]
    public async Task<IActionResult> Create(CreateUserViewModel model)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is not null && user.Email == Environment.GetEnvironmentVariable("ADMIN_EMAIL"))
        {
            return View(model);
        }

        TempData["Alert"] = "You're not allowed to create a new user!";
        return RedirectToAction("Index", user);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserViewModel model)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        TempData["IsAuthenticated"] = currentUser is not null;
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
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                };

                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    TempData["Message"] = "User created successfully!";
                    return RedirectToAction("Index", currentUser);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User creation failed.");
                }
            }

            return View("Create", model);
        }

        TempData["Alert"] = "You're not allowed to create a new user!";
        return RedirectToAction("Index", currentUser);
    }

    [HttpGet, Authorize]
    public async Task<IActionResult> Delete(string id)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        TempData["IsAuthenticated"] = currentUser is not null;
        if (currentUser is not null && currentUser.Email == Environment.GetEnvironmentVariable("ADMIN_EMAIL"))
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                View(user);
            }
            TempData["Alert"] = $"User {id} does not exist!";
            return RedirectToAction("ListUsers", currentUser);
        }

        TempData["Alert"] = "You're not allowed to delete a user!";
        return RedirectToAction("Index", currentUser);
    }

    [HttpPost, Authorize]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        TempData["IsAuthenticated"] = currentUser is not null;
        if (currentUser is not null && currentUser.Email == Environment.GetEnvironmentVariable("ADMIN_EMAIL"))
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    TempData["Message"] = $"User {user.FirstName} {user.LastName} was successfully deleted!";
                    return RedirectToAction("ListUsers", currentUser);
                }
                else
                {
                    Console.WriteLine($"Error deleting user: {result.Errors}");
                    TempData["Alert"] = $"An error occurred while attempting to delete user {user.FirstName} {user.LastName}!";
                    return RedirectToAction("ListUsers", currentUser);
                }
            }
            TempData["Alert"] = $"User {user.FirstName} {user.LastName} does not exist!";
            return RedirectToAction("ListUsers", currentUser);
        }

        TempData["Alert"] = "You're not allowed to delete a user!";
        return RedirectToAction("Index", currentUser);
    }

    [HttpGet, Authorize]
    public async Task<IActionResult> EditUser(string id)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        TempData["IsAuthenticated"] = currentUser is not null;
        if (currentUser is not null && currentUser.Email == Environment.GetEnvironmentVariable("ADMIN_EMAIL"))
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["Alert"] = $"User {id} does not exist!";
                return RedirectToAction("ListUsers", currentUser);
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

        TempData["Alert"] = "You're not allowed to edit a user!";
        return RedirectToAction("Index", currentUser);
    }

    [HttpPost, Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUser(EditUserViewModel model)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        TempData["IsAuthenticated"] = currentUser is not null;
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
                        TempData["Message"] = $"User {user.FirstName} {user.LastName} updated successfully!";
                        return RedirectToAction("ListUsers", currentUser);
                    }
                    else
                    {
                        Console.WriteLine($"Error updating user {user.Id}: {result.Errors}");
                        TempData["Alert"] = $"An error occurred while attempting to update user {user.FirstName} {user.LastName}!";
                        return RedirectToAction("ListUsers", currentUser);
                    }
                }
            }

            return View(model);
        }

        TempData["Alert"] = "You're not allowed to edit a user!";
        return RedirectToAction("Index", currentUser);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        TempData["Message"] = "Logout Successful!";
        return RedirectToAction("Index", "Home");
    }
}