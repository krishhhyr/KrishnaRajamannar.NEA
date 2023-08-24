namespace KrishnaRajamannar.NEA.Services
{
    public interface IIndependentReviewQuizService
    {
        void InsertTextQuestionQuizFeedback(int textQuestionID, int points, int quizID);

        void InsertMultipleChoiceQuestionQuizFeedback(int MCquestionID, int points, int quizID);


    }
}