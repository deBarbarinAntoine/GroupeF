using Microsoft.AspNetCore.Mvc;
using QuizApp.Models;
using QuizApp.Services;

namespace QuizApp.Controllers
{
    public class QuizController : Controller
    {
        private readonly QuestionService _questionService;
        private int _score = 0;

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
        public IActionResult SubmitAnswer(string answer, int correctAnswerIndex)
        {
            if (int.Parse(answer) == correctAnswerIndex)
            {
                _score++;
            }

            TempData["Score"] = _score;
            return RedirectToAction("Result");
        }

        public IActionResult Result()
        {
            var model = new ResultViewModel
            {
                UserName = TempData["UserName"]?.ToString(),
                Score = (int)(TempData["Score"] ?? 0)
            };
            return View(model);
        }
    }
}
