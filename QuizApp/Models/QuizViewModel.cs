namespace QuizApp.Models
{
    public class QuizViewModel
    {
        public Question Question { get; set; } = new Question();
        public string UserName { get; set; } = string.Empty;
    }
}
