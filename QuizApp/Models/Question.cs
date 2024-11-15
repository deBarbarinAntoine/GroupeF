namespace SimpleQuizApp.Models
{
    public class Question
    {
        public string Text { get; set; } = string.Empty;
        public string[] Options { get; set; } = Array.Empty<string>();
        public int CorrectAnswerIndex { get; set; }
    }
}
