using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YellowDirectory.Models;

namespace YellowDirectory.Controllers;

/// <summary>
/// AccountController manages all routes regarding the user:
/// Login, Logout, Dashboard, Manage users, Edit user, Delete user...
/// </summary>
public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    /// <summary>
    /// User Dashboard route
    /// </summary>
    /// <param name="user">the currently connected user</param>
    /// <returns>the dashboard view</returns>
    [HttpGet, Authorize]
    public async Task<IActionResult> Index(ApplicationUser user)
    {
        user = await _userManager.GetUserAsync(User);
        if (user is null)
        {
            // Handle case where user is not logged in (optional)
            return RedirectToAction("Login");
        }

        TempData["IsAdmin"] = user.Email == Environment.GetEnvironmentVariable("ADMIN_EMAIL");
        TempData["IsAuthenticated"] = true;

        return View(user);
    }

    /// <summary>
    /// Login route
    /// </summary>
    /// <returns>the login view</returns>
    [HttpGet]
    public async Task<IActionResult> Login()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is not null)
            return RedirectToAction("Index", user);

        return View();
    }

    /// <summary>
    /// Login treatment route
    /// </summary>
    /// <param name="model">the user credentials</param>
    /// <returns>redirect to the user dashboard route</returns>
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

            if (result.Succeeded)
            {
                var user = await _userManager.GetUserAsync(User);
                TempData["Message"] = $"Welcome {user.FirstName} {user.LastName}!";
                return RedirectToAction("Index", user);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
        }

        return View(model);
    }

    /// <summary>
    /// Manage users route
    /// </summary>
    /// <returns>the manage users page with the list of existing users</returns>
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

    /// <summary>
    /// Create user route
    /// </summary>
    /// <returns>the create user view</returns>
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is not null && user.Email == Environment.GetEnvironmentVariable("ADMIN_EMAIL"))
        {
            return View();
        }

        TempData["Alert"] = "You're not allowed to create a new user!";
        return RedirectToAction("Index", user);
    }

    /// <summary>
    /// Create user treatment route
    /// </summary>
    /// <param name="model">the user to create</param>
    /// <returns>redirect to the user dashboard route</returns>
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

    /// <summary>
    /// Delete user route
    /// </summary>
    /// <param name="id">the id of the user to delete</param>
    /// <returns>the delete user view</returns>
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
                var viewModel = new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    CreatedDate = user.CreatedDate
                };

                TempData["Alert"] = $"Do you want to delete user {id}?";
                return View(viewModel);
            }
            TempData["Alert"] = $"User {id} does not exist!";
            return RedirectToAction("ListUsers", currentUser);
        }

        TempData["Alert"] = "You're not allowed to delete a user!";
        return RedirectToAction("Index", currentUser);
    }

    /// <summary>
    /// Delete user treatment route
    /// </summary>
    /// <param name="id">the id of the user to delete</param>
    /// <returns>redirect to the manage users route</returns>
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
            TempData["Alert"] = $"User {id} does not exist!";
            return RedirectToAction("ListUsers", currentUser);
        }

        TempData["Alert"] = "You're not allowed to delete a user!";
        return RedirectToAction("Index", currentUser);
    }

    /// <summary>
    /// Edit user route
    /// </summary>
    /// <param name="id">the id of the user to edit</param>
    /// <returns>the edit user view</returns>
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

    /// <summary>
    /// Edit user treatment route
    /// </summary>
    /// <param name="model">the updated user</param>
    /// <returns>redirect to the manage users route</returns>
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

    /// <summary>
    /// Logout route
    /// </summary>
    /// <returns>redirect to the landing page</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        TempData["Message"] = "Logout Successful!";
        return RedirectToAction("Index", "Home");
    }
}