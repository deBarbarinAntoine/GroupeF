using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json; // Ajoutez ce namespace pour la sérialisation
using QuizApp.Models;
using QuizApp.Services;

namespace QuizApp.Controllers
{
    public class QuizController : Controller
    {
        private readonly QuestionService _questionService;
        private List<Question> _remainingQuestions;

        public QuizController()
        {
            _questionService = new QuestionService();
            _remainingQuestions = _questionService.GetAllQuestions();
        }

        [HttpGet]
        public IActionResult Start()
        {
            TempData.Clear(); // Réinitialiser TempData
            return View();
        }

        [HttpPost]
        public IActionResult Start(string userName)
        {
            TempData["UserName"] = userName;
            TempData["Score"] = 0;
            TempData["RemainingQuestions"] = JsonConvert.SerializeObject(_remainingQuestions);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (TempData["RemainingQuestions"] is string remainingQuestionsJson)
            {
                _remainingQuestions = JsonConvert.DeserializeObject<List<Question>>(remainingQuestionsJson) ?? new List<Question>();
            }

            if (_remainingQuestions == null || _remainingQuestions.Count == 0)
            {
                return RedirectToAction("Result");
            }

            var question = _remainingQuestions[0]; // Prendre la première question
            _remainingQuestions.RemoveAt(0); // Retirer la question actuelle
            TempData["RemainingQuestions"] = JsonConvert.SerializeObject(_remainingQuestions);
            TempData["CurrentQuestion"] = JsonConvert.SerializeObject(question);

            var model = new QuizViewModel
            {
                Question = question,
                UserName = TempData.Peek("UserName")?.ToString() ?? "Unknown"
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult SubmitAnswer(int answerIndex)
        {
            if (TempData["CurrentQuestion"] is string questionJson && !string.IsNullOrEmpty(questionJson))
            {
                var question = JsonConvert.DeserializeObject<Question>(questionJson);
                if (question != null)
                {
                    int score = (int)(TempData["Score"] ?? 0);
                    if (answerIndex == question.CorrectAnswerIndex)
                    {
                        score++;
                    }

                    TempData["Score"] = score;
                    TempData["LastQuestion"] = JsonConvert.SerializeObject(question); // Serialize pour TempData
                    TempData["LastAnswerIndex"] = answerIndex;
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult Quit()
        {
            return RedirectToAction("Result");
        }

public IActionResult Result()
{
    TempData.Keep();

    Question? lastQuestion = null;
    if (TempData["LastQuestion"] != null && TempData["LastQuestion"] is string lastQuestionJson && !string.IsNullOrEmpty(lastQuestionJson))
    {
        lastQuestion = JsonConvert.DeserializeObject<Question>(lastQuestionJson);
    }

    var lastAnswerIndex = (int?)TempData["LastAnswerIndex"];
    var score = (int)(TempData["Score"] ?? 0);

    var model = new ResultViewModel
    {
        UserName = TempData["UserName"]?.ToString() ?? "Unknown",
        Score = score
    };

    ViewBag.LastQuestion = lastQuestion;
    ViewBag.LastAnswerIndex = lastAnswerIndex;

    return View(model);
}

    }
}
