using KrishnaRajamannar.NEA.Models;
using System.Collections.Generic;

namespace KrishnaRajamannar.NEA.Services
{
    public interface IQuizService
    {
        public IList<QuizModel> GetQuiz(int? _userID);

        public void CreateQuiz(int? userID, string quizTitle);

        public void DeleteQuiz(int quizID, string quizTitle, int userID);

        public bool IsQuizExists(int userID, string quizTitleInput);

        public int? GetQuizID(int userID, string quizTitle);

        public void UpdateNumberOfQuestions(int numberOfQuestions, int quizID);


    }
}