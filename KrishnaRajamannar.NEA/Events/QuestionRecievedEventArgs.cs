using KrishnaRajamannar.NEA.Models;
using System;

namespace KrishnaRajamannar.NEA.Events
{
    // This is used to communciate with the View
    // to update the user interface based on the question type of the question that was recieved
    // when reviewing a quiz with other users
    public class QuestionRecievedEventArgs
    {
        public QuestionModel QuestionData { get; set; }
    }
    public delegate void QuestionRecievedEventHandler(Object sender, QuestionRecievedEventArgs e);
}
