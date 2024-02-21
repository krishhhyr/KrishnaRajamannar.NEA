using KrishnaRajamannar.NEA.Models;
using System.Collections.Generic;

namespace KrishnaRajamannar.NEA.Services.Interfaces
{
    public interface IMultiplayerReviewQuizService
    {
        public void InsertMultiplayerQuizFeedbackData(int sessionID, int userID, int quizID, string question, string answer, bool answeredCorrectly);
        public IList<MultiplayerReviewQuizFeedbackModel> GetMultiplayerQuizFeedbackForUser(int userID);
    }
}