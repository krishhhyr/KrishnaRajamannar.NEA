using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.Models
{
    public class MultiplayerReviewQuizFeedbackModel
    {
        public int QuestionNumber { get; set; }
        public string Question { get; set; }

        public string Answer { get; set; }
        public bool AnsweredCorrectly { get; set; }
    }
}
