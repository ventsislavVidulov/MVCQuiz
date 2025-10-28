namespace MVCQuiz.Models
{
    public class QuizResultViewModel
    { 
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public int Percentage { get; set; }
        public List<QuestionResult> Results { get; set; }
    }
}
