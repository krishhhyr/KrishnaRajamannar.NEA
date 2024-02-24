using System;

namespace KrishnaRajamannar.NEA.Events
{
    // This is used to notify the client and the session that the timer has finished
    // It is used to disable the UI elements in order to prevent users from submitting another answer to
    // a question. 
    public class TimerEventArgs
    {
        public bool TimerFinished { get; set; }
    }
    public delegate void TimerEventHandler(Object sender, TimerEventArgs e);
}
