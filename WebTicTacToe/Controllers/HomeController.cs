using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WebTicTacToe.Models;

namespace WebTicTacToe.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("gameContext") is null) return View(new GameContext());
        var gameContext = JsonSerializer.Deserialize<GameContext>(HttpContext.Session.GetString("gameContext") ?? string.Empty);
        
        // DEBUG
        // Console.WriteLine($"gameContext:\n{gameContext}");
        
        return View(gameContext);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}