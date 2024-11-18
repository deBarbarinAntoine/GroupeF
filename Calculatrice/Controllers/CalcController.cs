using Calculatrice.Models;
using Microsoft.AspNetCore.Mvc;

namespace Calculatrice.Controllers;

public class CalcController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View(new Calc());
    }

    [HttpPost]
    public IActionResult Index(Calc c, string calculate)
    {
        int total = 0;

        if (calculate == "add")
        {
            total = c.NumOne + c.NumTwo;
        }
        else if (calculate == "minus")
        {
            total = c.NumOne - c.NumTwo;
        }
        else if (calculate == "substract")
        {
            total = c.NumOne / c.NumTwo;
        }
        else if (calculate == "multiply")
        {
            total = c.NumOne * c.NumTwo;
        }

        var result = new Calc
        {
            NumOne = c.NumOne,
            NumTwo = c.NumTwo,
            Total = total
        };

        return View(result);
    }
}
