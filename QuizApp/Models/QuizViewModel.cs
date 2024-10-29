namespace QuizApp.Models
{
    public class QuizViewModel
    {
        public Question Question { get; set; }
        public string UserName { get; set; }
    }

    public class ResultViewModel
    {
        public string UserName { get; set; }
        public int Score { get; set; }
    }
}
