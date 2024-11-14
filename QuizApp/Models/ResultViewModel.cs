namespace QuizApp.Models
{
    public class ResultViewModel
    {
        public string UserName { get; set; }
        public int Score { get; set; }

        public ResultViewModel()
        {
            UserName = string.Empty; 
            Score = 0;           
        }
    }
}
