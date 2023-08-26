using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.Models
{
    public class IndependentReviewQuizModel
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public string? Option1 { get; set; }
        public string? Option2 { get; set; }
        public string? Option3 { get; set; }
        public string? Option4 { get; set; }
        public string? Option5 { get; set; }
        public string? Option6 { get; set; }
        public int PointsForQuestion { get; set; }
        public int Points { get; set; }
        public bool IsCorrect { get; set; }
        public int AnswerStreak { get; set; }
    }
}
