namespace KrishnaRajamannar.NEA.Services.Interfaces
{
    public interface IMultiplayerReviewQuizService
    {
        public void InsertMultiplayerQuizFeedbackData(int sessionID, int userID, string question, string answer, bool answeredCorrectly);
    }
}