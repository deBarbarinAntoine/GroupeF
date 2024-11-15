namespace SimpleQuizApp.Models
{
    public class QuizViewModel
    {
        public Question Question { get; set; } = new Question();
        public int CurrentQuestionIndex { get; set; }
        public int TotalQuestions { get; set; }
        public int Score { get; set; }
    }
}
