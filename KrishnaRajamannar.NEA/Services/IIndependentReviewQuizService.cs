using KrishnaRajamannar.NEA.Models;
using System.Collections.Generic;

namespace KrishnaRajamannar.NEA.Services
{
    public interface IIndependentReviewQuizService
    {
        void InsertTextQuestionQuizFeedback(int textQuestionID, int points, int quizID);

        void InsertMultipleChoiceQuestionQuizFeedback(int MCquestionID, int points, int quizID);

        public IList<IndependentReviewQuizModel> GetAllQuestions(int quizID);
    }
}