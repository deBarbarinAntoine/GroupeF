using Newtonsoft.Json;
using QuizApp.Models;
using System.Collections.Generic;
using System.IO;

namespace QuizApp.Services
{
   public class QuestionService
{
    private readonly string _filePath = "Data/questions.json";
    private List<Question> _questions = new List<Question>();

    public QuestionService()
    {
        if (File.Exists(_filePath))
        {
            var json = File.ReadAllText(_filePath);
            _questions = JsonConvert.DeserializeObject<List<Question>>(json) ?? new List<Question>();
        }
    }

        public Question GetRandomQuestion()
        {
            var random = new Random();
            return _questions[random.Next(_questions.Count)];
        }
    }
}