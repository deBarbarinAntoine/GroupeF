using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuizApp2.Models;
using System.IO;

namespace QuizApp2.Controllers
{
    public class QuizController : Controller
    {
        private static List<Question> _questions = new List<Question>();
        private static int _currentQuestionIndex = 0;
        private static int _score = 0;

        public QuizController()
        {
            if (_questions.Count == 0)
            {
                var questionsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Data/questions.json");
                if (System.IO.File.Exists(questionsFilePath))
                {
                    var json = System.IO.File.ReadAllText(questionsFilePath);
                    _questions = JsonConvert.DeserializeObject<List<Question>>(json) ?? new List<Question>();

                    if (_questions.Count == 0)
                    {
                        Console.WriteLine("Aucune question chargée : le fichier JSON est vide ou mal formaté.");
                    }
                }
                else
                {
                    Console.WriteLine($"Fichier introuvable : {questionsFilePath}");
                }
            }
        }

        [HttpGet]
        public IActionResult Start()
        {
            _currentQuestionIndex = 0;
            _score = 0;

            if (_questions.Count == 0)
            {
                return Content("Erreur : Aucune question disponible. Vérifiez le fichier JSON dans wwwroot/Data/questions.json.");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Question()
        {
            if (_currentQuestionIndex >= _questions.Count)
            {
                return RedirectToAction("Result");
            }

            var question = _questions[_currentQuestionIndex];
            return View(question);
        }

        [HttpPost]
        public IActionResult SubmitAnswer(int answerIndex)
        {
            if (_currentQuestionIndex < _questions.Count)
            {
                var question = _questions[_currentQuestionIndex];
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
            ViewBag.TotalQuestions = _questions.Count;
            return View();
        }

        [HttpPost]
        public IActionResult Quit()
        {
            return RedirectToAction("Result");
        }
    }
}
