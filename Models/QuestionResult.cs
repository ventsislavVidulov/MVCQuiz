namespace MVCQuiz.Models
{
    public class QuestionResult
    {
        public Question Question { get; set; }
        public string UserAnswer { get; set; }
        public bool IsCorrect { get; set; }
    }
}
