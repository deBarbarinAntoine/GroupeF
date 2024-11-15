using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace QuizApp.Controllers
{
    public class QuizController : Controller
    {
        private static readonly List<Question> Questions = new List<Question>
        {
            new Question { Text = "What is the capital of France?", Options = new[] { "Paris", "Berlin", "Madrid", "Rome" }, CorrectAnswerIndex = 0 },
            new Question { Text = "What is 2 + 2?", Options = new[] { "3", "4", "5", "6" }, CorrectAnswerIndex = 1 },
            new Question { Text = "Which planet is known as the Red Planet?", Options = new[] { "Earth", "Mars", "Jupiter", "Venus" }, CorrectAnswerIndex = 1 }
        };

        private static int _currentQuestionIndex = 0;
        private static int _score = 0;

        [HttpGet]
        public IActionResult Start()
        {
            _currentQuestionIndex = 0;
            _score = 0;
            return View();
        }

        [HttpGet]
        public IActionResult Question()
        {
            if (_currentQuestionIndex >= Questions.Count)
            {
                return RedirectToAction("Result");
            }

            var question = Questions[_currentQuestionIndex];
            return View(question);
        }

        [HttpPost]
        public IActionResult SubmitAnswer(int answerIndex)
        {
            if (_currentQuestionIndex < Questions.Count)
            {
                var question = Questions[_currentQuestionIndex];
                if (answerIndex == question.CorrectAnswerIndex)
                {
                    _score++;
                }

                _currentQuestionIndex++;
            }

            return RedirectToAction("Question");
        }

        [HttpGet]
        public IActionResult Result()
        {
            ViewBag.Score = _score;
            ViewBag.TotalQuestions = Questions.Count;
            return View();
        }

        [HttpPost]
public IActionResult Quit()
{
    // Redirige directement vers la page des résultats
    return RedirectToAction("Result");
}

    }

    public class Question
    {
        public string Text { get; set; } = string.Empty;
        public string[] Options { get; set; } = Array.Empty<string>();
        public int CorrectAnswerIndex { get; set; }
    }
}
