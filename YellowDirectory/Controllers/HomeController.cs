using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YellowDirectory.Models;

namespace YellowDirectory.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {

        var user = await _userManager.GetUserAsync(User);
        TempData["IsAuthenticated"] = user is not null;

        return View();
    }

    public async Task<IActionResult> Privacy()
    {

        var user = await _userManager.GetUserAsync(User);
        TempData["IsAuthenticated"] = user is not null;

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> Error()
    {

        var user = await _userManager.GetUserAsync(User);
        TempData["IsAuthenticated"] = user is not null;

        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}