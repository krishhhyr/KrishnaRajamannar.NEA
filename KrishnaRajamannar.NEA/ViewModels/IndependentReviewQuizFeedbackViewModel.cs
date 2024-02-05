using KrishnaRajamannar.NEA.Events;
using KrishnaRajamannar.NEA.Models;
using KrishnaRajamannar.NEA.Services;
using System.Collections.Generic;

namespace KrishnaRajamannar.NEA.ViewModels
{
    public class IndependentReviewQuizFeedbackViewModel 
    {
        public event HideWindowEventHandler HideIndependentReviewFeedbackWindow;
        private readonly IIndependentReviewQuizService _independentReviewQuizService;

        // Used so that the feedback can be retrieved from the database for that particular quiz
        // This data is passed onwards from either the ViewQuizzes window or the IndependentReviewQuiz window
        public int QuizID;

        public IndependentReviewQuizFeedbackViewModel(IIndependentReviewQuizService independentReviewQuizService)
        {
            _independentReviewQuizService = independentReviewQuizService;
        }

        // This is used to hide IndependentReviewFeedback window
        public void HideIndependentReviewFeedback()
        {
            HideWindowEventArgs args = new HideWindowEventArgs();
            args.IsHidden = true;
            OnHideIndependentReviewFeedbackWindow(args);
        }

        protected virtual void OnHideIndependentReviewFeedbackWindow(HideWindowEventArgs e)
        {
            HideWindowEventHandler handler = HideIndependentReviewFeedbackWindow;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        // This is used to return the feedback for the quiz that was attempted by users 
        public IList<IndependentReviewQuizFeedbackModel> GetQuizFeedback() 
        {
            IList<IndependentReviewQuizFeedbackModel> feedback = _independentReviewQuizService.GetQuizFeedback(QuizID);

            return feedback;
        }
    }
}
