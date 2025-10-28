using Microsoft.AspNetCore.Mvc;
using MVCQuiz.Models;
using MVCQuiz.Services;
using System.Diagnostics;

namespace MVCQuiz.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Services.QuizDataService _quizDataService;
        private Dictionary<int, string> result;

        public HomeController(ILogger<HomeController> logger, QuizDataService quizDataService)
        {
            _logger = logger;
            _quizDataService = quizDataService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Quiz()
        {
            return View(_quizDataService.defaultQuiz);
        }

        //[HttpPost]
        //public IActionResult SubmitQuiz(string[] selectedOption)
        //{
        //    result = selectedOption;
        //    Console.WriteLine("Seleced options:");
        //    foreach (var selected in selectedOption)
        //    {
        //        Console.WriteLine($"Selected option: {selected}");
        //    }

        //    return result != null ? RedirectToAction("QuizResult") : RedirectToAction("Quiz");
        //}

        //[HttpPost]
        //public IActionResult SubmitQuiz(Dictionary<int, string> selectedOption)
        //{
        //    if (selectedOption != null)
        //    {
        //        result = selectedOption;

        //        Console.WriteLine("Selected options:");
        //        foreach (var option in result)
        //        {
        //            Console.WriteLine($"Question {option.Key}: {option.Value}");
        //        }
        //    }

        //    return result != null ? RedirectToAction("QuizResult") : RedirectToAction("Quiz");
        //}

        //[HttpGet]

        //public IActionResult QuizResult()
        //{
        //    return View(result);
        //}

        [HttpPost]
        public IActionResult SubmitQuiz(Dictionary<int, string> selectedOption)
        {
            if (selectedOption != null && selectedOption.Count > 0)
            {
                TempData["QuizAnswers"] = System.Text.Json.JsonSerializer.Serialize(selectedOption);

                Console.WriteLine("Selected options:");
                foreach (var option in selectedOption)
                {
                    Console.WriteLine($"Question {option.Key}: {option.Value}");
                }

                return RedirectToAction("QuizResult");
            }
            else
            {
                TempData["Error"] = "Please answer all questions before submitting.";
                return RedirectToAction("Quiz");
            }
        }

        public IActionResult QuizResult()
        {
            var answersJson = TempData["QuizAnswers"] as string;

            if (string.IsNullOrEmpty(answersJson))
            {
                TempData["Error"] = "No quiz results found. Please take the quiz first.";
                return RedirectToAction("Quiz");
            }

            var selectedOption = System.Text.Json.JsonSerializer.Deserialize<Dictionary<int, string>>(answersJson);
            var quiz = _quizDataService.defaultQuiz;


            int score = 0;
            var results = new List<QuestionResult>();

            foreach (var answer in selectedOption)
            {
                var questionIndex = answer.Key;
                if (questionIndex < quiz.Questions.Count)
                {
                    var question = quiz.Questions[questionIndex];
                    var userAnswer = answer.Value;
                    var isCorrect = userAnswer == question.Answer;

                    if (isCorrect) score++;

                    results.Add(new QuestionResult
                    {
                        Question = question,
                        UserAnswer = userAnswer,
                        IsCorrect = isCorrect
                    });
                }
            }

            var viewModel = new QuizResultViewModel
            {
                Score = score,
                TotalQuestions = quiz.Questions.Count,
                Results = results,
                Percentage = (score * 100) / quiz.Questions.Count
            };

            return View(viewModel);
        }

        [HttpGet]

        public IActionResult Question()
        {
            var question = _quizDataService.GetRandomQuestion();
            var remaining = _quizDataService.GetRemainingQuestionsCount();
            Console.WriteLine(remaining);
            return View(question);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
