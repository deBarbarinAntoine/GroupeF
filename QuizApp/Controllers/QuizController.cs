using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SimpleQuizApp.Models;

namespace SimpleQuizApp.Controllers
{
    public class QuizController : Controller
    {
        private readonly List<Question> _questions;

        public QuizController()
        {
            // Charger les questions depuis un fichier JSON
            var questionsJson = System.IO.File.ReadAllText("Data/questions.json");
            _questions = JsonConvert.DeserializeObject<List<Question>>(questionsJson) ?? new List<Question>();
        }

        [HttpGet]
        public IActionResult Start()
        {
            TempData.Clear();
            return View();
        }

        [HttpPost]
        public IActionResult StartQuiz()
        {
            TempData["Score"] = 0;
            TempData["CurrentQuestionIndex"] = 0;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Index()
        {
            var currentIndex = (int)(TempData["CurrentQuestionIndex"] ?? 0);
            TempData.Keep();

            if (currentIndex >= _questions.Count)
                return RedirectToAction("Result");

            var model = new QuizViewModel
            {
                Question = _questions[currentIndex],
                CurrentQuestionIndex = currentIndex + 1,
                TotalQuestions = _questions.Count,
                Score = (int)(TempData["Score"] ?? 0)
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult SubmitAnswer(int answerIndex)
        {
            var currentIndex = (int)(TempData["CurrentQuestionIndex"] ?? 0);

            if (currentIndex < _questions.Count)
            {
                var question = _questions[currentIndex];

                if (answerIndex == question.CorrectAnswerIndex)
                {
                    TempData["Score"] = (int)(TempData["Score"] ?? 0) + 1;
                }
            }

            TempData["CurrentQuestionIndex"] = currentIndex + 1;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Result()
        {
            var model = new QuizViewModel
            {
                Score = (int)(TempData["Score"] ?? 0),
                TotalQuestions = _questions.Count
            };

            return View(model);
        }
    }
}
