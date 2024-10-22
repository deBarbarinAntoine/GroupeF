using Microsoft.AspNetCore.Mvc;
using QuizApp.Models;
using QuizApp.Services;

namespace QuizApp.Controllers
{
    public class QuizController : Controller
    {
        private readonly QuestionService _questionService;

        public QuizController()
        {
            _questionService = new QuestionService();
        }

        [HttpGet]
        public IActionResult Start()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Start(string userName)
        {
            TempData["UserName"] = userName;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Index()
        {
            var question = _questionService.GetRandomQuestion();
            var model = new QuizViewModel
            {
                Question = question,
                UserName = TempData["UserName"]?.ToString()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult SubmitAnswer(string answer, string correctAnswerIndex)
        {
            if (answer == correctAnswerIndex)
            {
                TempData["Correct"] = true;
            }
            else
            {
                TempData["Correct"] = false;
            }
            return RedirectToAction("Result");
        }

        public IActionResult Result()
        {
            var model = new ResultViewModel
            {
                UserName = TempData["UserName"]?.ToString(),
                IsCorrect = (bool)TempData["Correct"]
            };
            return View(model);
        }
    }
}
