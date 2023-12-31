using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.Models
{
    // A class which defines the properties of a quiz.
    [Serializable]
    public class QuizModel
    {
        public int QuizID { get; set; }
        public string QuizTitle { get; set; }
        public int NumberOfQuestions { get; set; }
        // Used to link the user who has created the quiz.
        public int UserID { get; set; }
    }
}
