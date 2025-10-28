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
            return View(_quizDataService.GetAllQuizzes());
        }

        public IActionResult Quiz(int id)
        {
            var quiz = _quizDataService.GetQuizById(id);
            if (quiz == null)
            {
                TempData["Error"] = $"Quiz '{id}' not found.";
                return RedirectToAction("Index");
            }
            //Console.WriteLine("here");

            return View(quiz);
        }

        public IActionResult SubmitQuiz(int quizId, Dictionary<int, string> selectedOption)
        {
            Console.WriteLine($"Received quizId: {quizId}");

            if (selectedOption != null && selectedOption.Count > 0)
            {
                //store quizId and answers
                TempData["QuizId"] = quizId;
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
                return RedirectToAction("Quiz", new { id = quizId });
            }
        }

        public IActionResult QuizResult()
        {
            //get quizId from TempData
            if (!TempData.ContainsKey("QuizId") || !TempData.ContainsKey("QuizAnswers"))
            {
                TempData["Error"] = "No quiz results found. Please take a quiz first.";
                return RedirectToAction("Index");
            }

            var quizId = Convert.ToInt32(TempData["QuizId"]);
            var answersJson = TempData["QuizAnswers"] as string;

            var selectedOption = System.Text.Json.JsonSerializer.Deserialize<Dictionary<int, string>>(answersJson);
            var quiz = _quizDataService.GetQuizById(quizId);

            if (quiz == null)
            {
                TempData["Error"] = "Quiz not found.";
                return RedirectToAction("Index");
            }

            //calculate results
            int score = 0;
            var results = new List<QuestionResultModel>();

            foreach (var answer in selectedOption)
            {
                var questionIndex = answer.Key;
                if (questionIndex < quiz.Questions.Count)
                {
                    var question = quiz.Questions[questionIndex];
                    var userAnswer = answer.Value;
                    var isCorrect = userAnswer == question.Answer;

                    if (isCorrect) score++;

                    results.Add(new QuestionResultModel
                    {
                        Question = question,
                        UserAnswer = userAnswer,
                        IsCorrect = isCorrect
                    });
                }
            }

            var viewModel = new QuizResultViewModel
            {
                QuizId = quizId,
                QuizTitle = quiz.Title,
                Score = score,
                TotalQuestions = quiz.Questions.Count,
                Results = results,
                Percentage = (score * 100) / quiz.Questions.Count
            };

            return View(viewModel);
        }

        //public IActionResult QuizResult()
        //{
        //    var answersJson = TempData["QuizAnswers"] as string;

        //    if (string.IsNullOrEmpty(answersJson))
        //    {
        //        TempData["Error"] = "No quiz results found. Please take the quiz first.";
        //        return RedirectToAction("Quiz");
        //    }

        //    var selectedOption = System.Text.Json.JsonSerializer.Deserialize<Dictionary<int, string>>(answersJson);
        //    var quiz = _quizDataService.defaultQuiz;


        //    int score = 0;
        //    var results = new List<QuestionResult>();

        //    foreach (var answer in selectedOption)
        //    {
        //        var questionIndex = answer.Key;
        //        if (questionIndex < quiz.Questions.Count)
        //        {
        //            var question = quiz.Questions[questionIndex];
        //            var userAnswer = answer.Value;
        //            var isCorrect = userAnswer == question.Answer;

        //            if (isCorrect) score++;

        //            results.Add(new QuestionResult
        //            {
        //                Question = question,
        //                UserAnswer = userAnswer,
        //                IsCorrect = isCorrect
        //            });
        //        }
        //    }

        //    var viewModel = new QuizResultViewModel
        //    {
        //        Score = score,
        //        TotalQuestions = quiz.Questions.Count,
        //        Results = results,
        //        Percentage = (score * 100) / quiz.Questions.Count
        //    };

        //    return View(viewModel);
        //}

        //[HttpGet]

        //public IActionResult Question(QuizModel quiz)
        //{
        //    var question = _quizDataService.GetRandomQuestion(quiz);
        //    var remaining = _quizDataService.GetRemainingQuestionsCount();
        //    Console.WriteLine(remaining);
        //    return View(question);
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
