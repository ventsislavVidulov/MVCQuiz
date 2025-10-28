using System.Text.Json.Serialization;

namespace MVCQuiz.Models
{
    public class QuizModel
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("questions")]
        public List<QuestionModel> Questions { get; set; }

        public QuizModel(string title, List<QuestionModel> questions)
        {
            Title = title;
            Questions = questions;
        }
    }
}
