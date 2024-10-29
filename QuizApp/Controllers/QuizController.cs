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
            TempData["Score"] = 0; // Initialiser le score dans TempData pour la persistance
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Index()
        {
            var question = _questionService.GetRandomQuestion();
            var model = new QuizViewModel
            {
                Question = question,
                UserName = TempData.Peek("UserName")?.ToString() // Utiliser Peek pour conserver UserName
            };
            return View(model);
        }

        [HttpPost]
        [HttpPost]
        public IActionResult SubmitAnswer(string answer, int correctAnswerIndex)
        {
            Console.WriteLine("Réponse utilisateur : " + answer);
            Console.WriteLine("Index correct : " + correctAnswerIndex);

            if (int.TryParse(answer, out int answerIndex) && answerIndex == correctAnswerIndex)
            {
                _score++;
                Console.WriteLine("Bonne réponse ! Score actuel : " + _score);
            }
            else
            {
                Console.WriteLine("Mauvaise réponse.");
            }

            TempData["Score"] = _score;
            return RedirectToAction("Result");
        }



        public IActionResult Result()
        {
            TempData.Keep(); // Conserver TempData pour cette requête

            var model = new ResultViewModel
            {
                UserName = TempData["UserName"]?.ToString() ?? string.Empty,
                Score = (int)(TempData["Score"] ?? 0)
            };
            return View(model);
        }
    }
}
