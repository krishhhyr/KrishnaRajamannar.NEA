using KrishnaRajamannar.NEA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.ViewModels
{
    public class IndependentReviewQuizFeebackViewModel
    {
        public IndependentReviewQuizService IndependentReviewQuizService { get; set; }

        public void GetData() 
        {
            IndependentReviewQuizService.GetQuizFeedback(36);
        }
    }
}
