using System;

namespace KrishnaRajamannar.NEA.Events
{
    // This is used to show a new window and pass data about the quiz that has been selected
    // Used as an example when showing the independent review quiz window after
    // selecting a quiz to review
    public class ShowQuizParameterWindowEventArgs
    {
        public bool IsShown { get; set; }
        public int QuizID { get; set; }

        public int UserID { get; set; }
    }
    public delegate void ShowQuizParameterWindowEventHandler(Object sender, ShowQuizParameterWindowEventArgs e);
}
