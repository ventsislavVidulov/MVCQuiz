using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MVCQuiz.Models
{
    public class Question
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Answer { get; set; }

        public string[] Options { get; set; }
    }
}
