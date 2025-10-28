﻿namespace MVCQuiz.Models
{
    public class QuestionModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Answer { get; set; }

        public string[] Options { get; set; }
    }
}
