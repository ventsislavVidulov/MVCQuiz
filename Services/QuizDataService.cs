using MVCQuiz.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MVCQuiz.Services
{
    public class QuizDataService
    {
        //private List<Question> randomizedQuestions;
        //private static readonly string _quizDataFile = "Data/MVCQuiz.json";
        //private QuizModel _deserializedQuiz = JsonConvert.DeserializeObject<QuizModel>(File.ReadAllText(_quizDataFile));
        //private QuizModel? _currentQuiz;
        private readonly IHostEnvironment _environment;
        private static readonly Random random = new Random();
        private List<QuizModel> _quizzes = new();

        //public QuizModel defaultQuiz;

        public QuizDataService(IHostEnvironment environment)
        {
            //defaultQuiz = _deserializedQuiz;
            _environment = environment;
            ReadAllJsonFiles();
        }
        private void ReadAllJsonFiles()
        {
            string dataPath = Path.Combine(_environment.ContentRootPath, "Data");
            string[] jsonFiles = Directory.GetFiles(dataPath, "*.json");
            int id = 1;

            foreach (string file in jsonFiles)
            {
                string jsonContent = File.ReadAllText(file);
                QuizModel currentQuiz = JsonConvert.DeserializeObject<QuizModel>(jsonContent);
                currentQuiz.Id = id++;

                _quizzes.Add(currentQuiz);

            }
            Console.WriteLine("All loaded quiz IDs:");
            foreach (var quiz in _quizzes)
            {
                Console.WriteLine($" - Quiz ID: {quiz.Id}, Title: {quiz.Title}");
            }
        }

        public List<QuizModel> GetAllQuizzes()
        {
            return _quizzes;
        }

        public QuizModel GetQuizById(int id)
        {
            Console.WriteLine($"Requested id {id}");
            if (id == 0)
            {
                //returns combined quiz for ID 0
                return GetDefaultQuiz();
            }

            QuizModel? quiz = _quizzes.FirstOrDefault(q => q.Id == id);
            if (quiz == null)
            {
                Console.WriteLine($"Quiz with ID {id} not found.");
            }

            RandomizeOptions(quiz);
            RandomizeQuestion(quiz);
            //_currentQuiz = quiz;
            return quiz;
        }

        //not working if two users start different quizzes
        //public QuizModel GetCurrentQuiz()
        //{
        //    if (_currentQuiz != null)
        //    {
        //        QuizModel quiz = _currentQuiz;
        //        _currentQuiz = null;
        //        return quiz;
        //    } 
        //    else
        //    {
        //        Console.WriteLine("There is no quiz started");
        //    }
        //    return null;
        //}

        public QuizModel GetDefaultQuiz()
        {
            //combine all quizzes into one
            var combinedQuestions = new List<QuestionModel>();
            int questionId = 1;

            foreach (var quiz in _quizzes)
            {
                foreach (var question in quiz.Questions)
                {
                    combinedQuestions.Add(new QuestionModel
                    {
                        Id = questionId++,
                        Title = $"[{quiz.Title}] {question.Title}",
                        Answer = question.Answer,
                        Options = question.Options
                    });
                }
            }

            QuizModel megaQuiz = new QuizModel("All Quizzes Combined", combinedQuestions);
            megaQuiz.Id = 0;
            return megaQuiz;
        }

        public void RandomizeQuestion(QuizModel quiz)
        {
            List<QuestionModel> listCopy = new List<QuestionModel>(quiz.Questions);

            for (int i = 0; i < 4; i++)
            {
                int j = random.Next(i, listCopy.Count);

                (listCopy[i], listCopy[j]) = (listCopy[j], listCopy[i]);
            }

            quiz.Questions = listCopy.ToList();
        }

        public void RandomizeOptions(QuizModel quiz)
        {
            foreach (var question in quiz.Questions)
            {
                var options = question.Options.ToList();

                for (int i = 0; i < options.Count; i++)
                {
                    int j = random.Next(i, options.Count);
                    (options[i], options[j]) = (options[j], options[i]);
                }

                question.Options = options.ToArray();
            }
        }

        //public Question GetRandomQuestion(Quiz quiz)
        //{
        //    if (randomizedQuestions == null || randomizedQuestions.Count == 0)
        //    {
        //        RandomizeQuestion(quiz);
        //    }

        //    Question question = randomizedQuestions[0];
        //    randomizedQuestions.RemoveAt(0);
        //    return question;
        //}

        //public int GetRemainingQuestionsCount()
        //{
        //    return randomizedQuestions?.Count ?? 0;
        //}
    }
}
