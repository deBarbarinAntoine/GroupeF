using Microsoft.AspNetCore.Mvc;
using WebTicTacToe.Models;
using static System.Text.Json.JsonSerializer;

namespace WebTicTacToe.Controllers;

/// <summary>
/// GameController is the Controller for all Game-related requests.
/// </summary>
public class GameController : Controller
{
    /// <summary>
    /// Game Index route, to initialize a new game.
    /// </summary>
    /// <returns>the Index view.</returns>
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Game initialization treatment route.
    /// </summary>
    /// <param name="gameConstructor">the game configuration sent by the Game/Index form.</param>
    /// <returns>Redirects to the Game page.</returns>
    [HttpPost]
    public IActionResult Start(GameConstructor gameConstructor)
    {
        Game game;
        try
        {
            // Create a new game
            game = Game.New(gameConstructor.SelectedMode, gameConstructor.PlayerName1, gameConstructor.PlayerName2);
        }
        catch (ArgumentException e)
        {
            // Catch and send errors back to the form
            ModelState.AddModelError(string.Empty, e.Message);
            return View("Index", gameConstructor);
        }

        // Convert the Game into a GameContext to store it in the Session Context
        GameContext gameContext = new GameContext(game);

        // Store the Game in the Session Context for further usage
        HttpContext.Session.SetString("gameContext", Serialize(gameContext));
        return Redirect("/Home/Index/");
    }

    /// <summary>
    /// AI playing route (used with AJAX).
    /// </summary>
    /// <returns>the new board with the AI's move.</returns>
    [HttpGet]
    public IActionResult Play()
    {
        // DEBUG
        Console.WriteLine("AI Turn...");

        // Retrieve the game from the Session Context & Check if it's okay
        GameContext? gameContext = Deserialize<GameContext>(HttpContext.Session.GetString("gameContext") ?? string.Empty);
        if (gameContext == null)
            return View("_GameError");
        
        if (gameContext.Empty)
            return View("_GameError");
        
        Game? game = gameContext.GetGame();
        if (game == null)
            return View("_GameError");

        // Do nothing if it's not the AI's turn
        if (game.CurrentPlayer().IsHuman) return View("BoardAJAX", game);

        // Make the AI play
        if (game.CurrentPlayer().AiPlay(game.Board))
        {
            Game.State currentState = game.CheckEndGame(game.CurrentPlayer().LastMove);
            game.NextTurn();
            
            HttpContext.Session.SetString("gameContext", Serialize(new GameContext(game)));
            
            return View(currentState == Game.State.Playing ? "BoardAJAX" : "EndGameAJAX", game);
        }

        // Error handling
        TempData["Alert"] = "IA Turn Failed!";
        return View("BoardAJAX", game);
    }

    /// <summary>
    /// Human Player route (used with AJAX)
    /// </summary>
    /// <param name="index">the index of the cell to play in.</param>
    /// <returns>the new board with the Human Player's move.</returns>
    [HttpPost]
    public IActionResult Play(int index)
    {
        // Check the id sent in JSON
        if (!Request.HasJsonContentType())
        {
            // Handle invalid request type
            return BadRequest("Invalid request type");
        }

        // Retrieve the index from the JSON
        var jsonObj = Request.ReadFromJsonAsync<IndexJson>().Result;
        if (jsonObj == null)
            return BadRequest("Error retrieving JSON");
        
        index = jsonObj.Index;
        
        // DEBUG
        Console.WriteLine($"index received: {index}");

        // Retrieve the game from the Session Context
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

        // Check the index
        if (index < 0 || index >= game.Board.Count)
        {
            // DEBUG
            Console.WriteLine($"Invalid index: {index}");
            TempData["Alert"] = "Invalid cell value!";
            return View("BoardAJAX", game);
        }

        // Return the game board without modification if it's not a Human Player's turn
        if (!game.CurrentPlayer().IsHuman) return View("BoardAJAX", game);

        // Check the cell to play in
        if (game.CurrentPlayer().Play(game.Board, index))
        {
            Game.State currentState = game.CheckEndGame(index);
            game.NextTurn();
            
            HttpContext.Session.SetString("gameContext", Serialize(new GameContext(game)));
            
            return View(currentState == Game.State.Playing ? "BoardAJAX" : "EndGameAJAX", game);
        }

        // DEBUG
        Console.WriteLine($"You can't play on cell nº{index}!");

        // Send error message if the cell isn't available
        TempData["Alert"] = "You can't play on this cell!";
        return View("BoardAJAX", game);
    }

    /// <summary>
    /// The Clear route clears the game from the Session Context.
    /// </summary>
    /// <returns>Redirects to /Home/Index.</returns>
    [HttpPost]
    public IActionResult Clear()
    {
        HttpContext.Session.Remove("gameContext");
        return Redirect($"/Home/Index");
    }

    /// <summary>
    /// The Restart route starts a new game with the same configurations.
    /// </summary>
    /// <returns>Redirects to the game page.</returns>
    [HttpPost]
    public IActionResult Restart()
    {

        // Retrieve the game from the Session Context
        GameContext? gameContext = Deserialize<GameContext>(HttpContext.Session.GetString("gameContext") ?? string.Empty);

        if (gameContext == null)
            return View("_GameError");

        if (gameContext.Empty)
            return View("_GameError");

        Game? game = gameContext.GetGame();
        if (game == null)
            return View("_GameError");

        // Initialize a new game with the same configurations
        game = Game.New(game.CurrentMode, game.Player1.Name, game.Player2.Name);

        // overwrite the Session Context with the new game
        HttpContext.Session.SetString("gameContext", Serialize(new GameContext(game)));

        return Redirect("/Home/Index/");
    }
}