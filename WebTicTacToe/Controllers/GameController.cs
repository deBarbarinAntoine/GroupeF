using Microsoft.AspNetCore.Mvc;
using WebTicTacToe.Models;
using static System.Text.Json.JsonSerializer;

namespace WebTicTacToe.Controllers;

public class GameController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Start(GameConstructor gameConstructor)
    {
        Game game = Game.New(gameConstructor.SelectedMode, gameConstructor.PlayerName1, gameConstructor.PlayerName2);
        GameContext gameContext = new GameContext(game);
        
        HttpContext.Session.SetString("gameContext", Serialize(gameContext));
        return Redirect("/Home/Index/");
    }

    [HttpGet]
    public IActionResult Play()
    {
        // DEBUG
        Console.WriteLine("IA Turn...");
        
        GameContext? gameContext = Deserialize<GameContext>(HttpContext.Session.GetString("gameContext") ?? string.Empty);
        if (gameContext == null)
            return View("_GameError");
        
        if (gameContext.Empty)
            return View("_GameError");
        
        Game? game = gameContext.GetGame();
        if (game == null)
            return View("_GameError");
        
        if (game.CurrentPlayer().IsHuman) return View("BoardAJAX", game);

        if (game.CurrentPlayer().IaPlay(game.Board))
        {
            Game.State currentState = game.CheckEndGame(game.CurrentPlayer().LastMove, game.CurrentPlayer());
            game.NextTurn();
            
            HttpContext.Session.SetString("gameContext", Serialize(new GameContext(game)));
            
            return View(currentState == Game.State.Playing ? "BoardAJAX" : "EndGameAJAX", game);
        }
        TempData["Alert"] = "IA Turn Failed!";
        return View("BoardAJAX", game);
    }

    [HttpPost]
    public IActionResult Play(int index)
    {
        if (!Request.HasJsonContentType())
        {
            // Handle invalid request type
            return BadRequest("Invalid request type");
        }

        var jsonObj = Request.ReadFromJsonAsync<IndexJson>().Result;
        if (jsonObj == null)
            return BadRequest("Error retrieving JSON");
        
        index = jsonObj.Index;
        
        // DEBUG
        Console.WriteLine($"index received: {index}");
        
        GameContext? gameContext = Deserialize<GameContext>(HttpContext.Session.GetString("gameContext") ?? string.Empty);
        if (gameContext == null)
        {
            // DEBUG
            // Console.WriteLine($"gameContext:\n{gameContext}");
            return View("_GameError");
        }
        
        if (gameContext.Empty)
        {
            // DEBUG
            // Console.WriteLine($"gameContext:\n{gameContext}");
            return View("_GameError");
        }
        
        Game? game = gameContext.GetGame();
        if (game == null)
        {
            // DEBUG
            // Console.WriteLine($"gameContext:\n{gameContext}");
            return View("_GameError");
        }

        if (index < 0 || index >= game.Board.Count)
        {
            // DEBUG
            Console.WriteLine($"Invalid index: {index}");
            TempData["Alert"] = "Invalid cell value!";
            return View("BoardAJAX", game);
        }
            
        if (!game.CurrentPlayer().IsHuman) return View("BoardAJAX", game);
        
        if (game.CurrentPlayer().Play(game.Board, index))
        {
            Game.State currentState = game.CheckEndGame(index, game.CurrentPlayer());
            game.NextTurn();
            
            HttpContext.Session.SetString("gameContext", Serialize(new GameContext(game)));
            
            return View(currentState == Game.State.Playing ? "BoardAJAX" : "EndGameAJAX", game);
        }
        // DEBUG
        Console.WriteLine($"You can't play on cell nº{index}!");
        TempData["Alert"] = "You can't play on this cell!";
        return View("BoardAJAX", game);
    }
}