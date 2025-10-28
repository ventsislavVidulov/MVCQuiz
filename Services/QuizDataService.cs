using MVCQuiz.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MVCQuiz.Services
{
    public class QuizDataService
    {
        private static readonly string QuizDataFile = "Data/MVCQuiz.json";
        Quiz deserializedQuiz = JsonConvert.DeserializeObject<Quiz>(File.ReadAllText(QuizDataFile));
        public Quiz defaultQuiz;
        private static readonly Random random = new Random();
        private List<Question> randomizedQuestions;
        public QuizDataService()
        {
            defaultQuiz = deserializedQuiz;
        }
        public void RandomizeQuestion()
        {
            List<Question> listCopy = new List<Question>(defaultQuiz.Questions);

            for (int i = 0; i < 4; i++)
            {
                int j = random.Next(i, listCopy.Count);

                (listCopy[i], listCopy[j]) = (listCopy[j], listCopy[i]);
            }

            randomizedQuestions = listCopy.Take(4).ToList();
        }

        public Question GetRandomQuestion()
        {
            if (randomizedQuestions == null || randomizedQuestions.Count == 0)
            {
                RandomizeQuestion();
            }

            Question question = randomizedQuestions[0];
            randomizedQuestions.RemoveAt(0);
            return question;
        }

        public int GetRemainingQuestionsCount()
        {
            return randomizedQuestions?.Count ?? 0;
        }
    }
}
