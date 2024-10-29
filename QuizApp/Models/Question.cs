namespace QuizApp.Models
{
    public class Question
    {
        public string Text { get; set; }
        public string[] Options { get; set; }
        public int CorrectAnswerIndex { get; set; }
    }
}
