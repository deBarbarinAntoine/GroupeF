namespace QuizApp.Models
{
    public class QuizViewModel
    {
        public Question Question { get; set; }
        public string UserName { get; set; }

        public QuizViewModel()
        {
            Question = new Question(); // Fournit une valeur par défaut pour éviter null.
            UserName = string.Empty;   // Assure que le nom d'utilisateur est non-null.
        }
    }
}
