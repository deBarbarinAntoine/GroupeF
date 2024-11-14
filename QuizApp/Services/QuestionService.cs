using Newtonsoft.Json;
using QuizApp.Models;
using System.Collections.Generic;
using System.IO;

namespace QuizApp.Services
{
    public class QuestionService
    {
        private readonly string _filePath = "Data/questions.json";
        private List<Question> _questions;

        public QuestionService()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _questions = JsonConvert.DeserializeObject<List<Question>>(json) ?? new List<Question>();
            }
            else
            {
                _questions = new List<Question>();
            }
        }

        public List<Question> GetAllQuestions()
        {
            return new List<Question>(_questions); // Copie pour éviter la modification
        }
    }
}
