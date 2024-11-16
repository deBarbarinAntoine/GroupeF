using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WebTicTacToe.Models;

namespace WebTicTacToe.Controllers;

/// <summary>
/// HomeController is the controller handling Index and Privacy routes.
/// </summary>
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// The Index route.
    /// </summary>
    /// <returns>the Index view.</returns>
    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("gameContext") is null) return View(new GameContext());
        var gameContext = JsonSerializer.Deserialize<GameContext>(HttpContext.Session.GetString("gameContext") ?? string.Empty);
        
        // DEBUG
        // Console.WriteLine($"gameContext:\n{gameContext}");
        
        return View(gameContext);
    }

    /// <summary>
    /// The Privacy route.
    /// </summary>
    /// <returns>the Privacy view.</returns>
    public IActionResult Privacy()
    {
        return View();
    }

    /// <summary>
    /// The Error route.
    /// </summary>
    /// <returns>the Error view.</returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}