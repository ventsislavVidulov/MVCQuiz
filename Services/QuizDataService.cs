using MVCQuiz.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MVCQuiz.Services
{
    public class QuizDataService
    {
        //private List<Question> randomizedQuestions;
        private readonly IHostEnvironment _environment;
        private static readonly string _quizDataFile = "Data/MVCQuiz.json";
        private static readonly Random random = new Random();
        private List<QuizModel> _quizzes = new();
        private QuizModel _deserializedQuiz = JsonConvert.DeserializeObject<QuizModel>(File.ReadAllText(_quizDataFile));

        public QuizModel defaultQuiz;

        public QuizDataService(IHostEnvironment environment)
        {
            _environment = environment;
            defaultQuiz = _deserializedQuiz;
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
                // Return combined quiz for ID 0
                return GetDefaultQuiz();
            }

            return _quizzes.FirstOrDefault(q => q.Id == id);
        }

        public QuizModel GetDefaultQuiz()
        {
            // Combine all quizzes into one
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


        //public void RandomizeQuestion(Quiz quiz)
        //{
        //    List<Question> listCopy = new List<Question>(quiz.Questions);

        //    for (int i = 0; i < 4; i++)
        //    {
        //        int j = random.Next(i, listCopy.Count);

        //        (listCopy[i], listCopy[j]) = (listCopy[j], listCopy[i]);
        //    }

        //    randomizedQuestions = listCopy.Take(4).ToList();
        //}

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
