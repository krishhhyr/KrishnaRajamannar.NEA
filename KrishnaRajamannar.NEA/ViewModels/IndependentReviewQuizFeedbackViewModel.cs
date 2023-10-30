using KrishnaRajamannar.NEA.Models;
using KrishnaRajamannar.NEA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.ViewModels
{
    public class IndependentReviewQuizFeedbackViewModel
    {
        private readonly IIndependentReviewQuizService _independentReviewQuizService;

        public int QuizID;

        public IndependentReviewQuizFeedbackViewModel(IIndependentReviewQuizService independentReviewQuizService)
        {
            _independentReviewQuizService = independentReviewQuizService;
        }

        public IList<IndependentReviewQuizFeedbackModel> GetQuizzes() 
        {
            IList<IndependentReviewQuizFeedbackModel> feedback = _independentReviewQuizService.GetQuizFeedback(QuizID);

            return feedback;
        }
    }
}
