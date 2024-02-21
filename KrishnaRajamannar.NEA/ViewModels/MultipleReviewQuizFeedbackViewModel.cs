using KrishnaRajamannar.NEA.Events;
using KrishnaRajamannar.NEA.Models;
using KrishnaRajamannar.NEA.Services;
using KrishnaRajamannar.NEA.Services.Database;
using KrishnaRajamannar.NEA.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.ViewModels
{
    public class MultipleReviewQuizFeedbackViewModel
    {
        public event HideWindowEventHandler HideMultipleReviewQuizFeedbackWindow;
        private readonly IMultiplayerReviewQuizService _multipleReviewQuizService;

        public int UserID;

        public MultipleReviewQuizFeedbackViewModel(IMultiplayerReviewQuizService multipleReviewQuizService)
        {
            _multipleReviewQuizService = multipleReviewQuizService;
        }

        public void HideMultipleReviewQuizFeedback()
        {
            HideWindowEventArgs args = new HideWindowEventArgs();
            args.IsHidden = true;
            OnHideMultipleReviewQuizFeedbackWindow(args);
        }

        protected virtual void OnHideMultipleReviewQuizFeedbackWindow(HideWindowEventArgs e)
        {
            HideWindowEventHandler handler = HideMultipleReviewQuizFeedbackWindow;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public IList<MultiplayerReviewQuizFeedbackModel> GetQuizFeedback()
        {
            IList<MultiplayerReviewQuizFeedbackModel> quizFeedback = _multipleReviewQuizService.GetMultiplayerQuizFeedbackForUser(UserID);

            return quizFeedback;
        }
    }
}
