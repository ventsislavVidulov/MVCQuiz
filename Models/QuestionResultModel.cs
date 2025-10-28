namespace MVCQuiz.Models
{
    public class QuestionResultModel
    {
        public QuestionModel Question { get; set; }
        public string UserAnswer { get; set; }
        public bool IsCorrect { get; set; }
    }
}
