using System;

namespace KrishnaRajamannar.NEA.Models
{
    // This means that the entire object can be converted into a byte stream
    [Serializable]
    // A class which defines the properties of a quiz.
    public class QuizModel
    {
        public int QuizID { get; set; }
        public string QuizTitle { get; set; }
        public int NumberOfQuestions { get; set; }
        // Used to link the user who has created the quiz.
        public int UserID { get; set; }
    }
}
