using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YellowDirectory.Models;

namespace YellowDirectory.Controllers;

/// <summary>
/// HomeController manages all basic routes.
/// </summary>
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    /// <summary>
    /// Index route
    /// </summary>
    /// <returns>the index view</returns>
    public async Task<IActionResult> Index()
    {

        var user = await _userManager.GetUserAsync(User);
        TempData["IsAuthenticated"] = user is not null;

        return View();
    }

    /// <summary>
    /// Privacy policy route
    /// </summary>
    /// <returns>the privacy policy view</returns>
    public async Task<IActionResult> Privacy()
    {

        var user = await _userManager.GetUserAsync(User);
        TempData["IsAuthenticated"] = user is not null;

        return View();
    }

    /// <summary>
    /// Error route
    /// </summary>
    /// <returns>the error view</returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> Error()
    {

        var user = await _userManager.GetUserAsync(User);
        TempData["IsAuthenticated"] = user is not null;

        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}